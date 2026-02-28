using UnityEngine;

public class SpookyChair : MonoBehaviour
{
    [Header("เป้าหมาย (ลากตัวละคร PlayerCapsule มาใส่)")]
    public Transform player;

    [Header("ระยะที่ผีจะดักรอ (ต้องเดินเข้าไปใกล้เก้าอี้ก่อน)")]
    public float armDistance = 3f;

    [Header("ระยะที่ผีจะออกแรงผลัก (เดินหนีออกมาไกลเท่านี้)")]
    public float triggerDistance = 4.5f;

    [Header("ความแรง (ยิ่งเยอะยิ่งกระเด็นไกล)")]
    public float pushForce = 10f; // 🌟 ปรับค่าเริ่มต้นให้แรงขึ้นเป็น 10

    [Header("เสียงตอนล้ม (ลาก Audio Source มาใส่)")]
    public AudioSource fallSound;

    private Rigidbody rb;
    private bool isArmed = false; // สวิตช์เช็คว่าผีเตรียมตัวหรือยัง
    private bool hasTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!hasTriggered && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // สเต็ป 1: ถ้าเดินเข้าไปใกล้เก้าอี้ปุ๊บ ผีจะเตรียมตัว (แต่ยังไม่ทำอะไร)
            if (!isArmed && distance <= armDistance)
            {
                isArmed = true;
                Debug.Log("ผีเตรียมตัว... รอจอนหันหลัง!");
            }

            // สเต็ป 2: ถ้าผีเตรียมตัวแล้ว และเราเดินห่างออกมาเกินระยะ... ผลัก!!
            if (isArmed && distance > triggerDistance)
            {
                ScarePlayer();
            }
        }
    }

    void ScarePlayer()
    {
        hasTriggered = true;

        if (rb != null)
        {
            // 🌟 ท่าไม้ตาย: สร้าง "จุดระเบิดจำลอง" ไว้ที่ใต้เก้าอี้ (เยื้องมาข้างหน้านิดนึง)
            Vector3 pointUnderChair = transform.position + (transform.forward * 0.5f) - (transform.up * 0.2f);

            // 🌟 สั่งระเบิดงัดเก้าอี้! (แรงบึ้ม, จุดเกิดระเบิด, รัศมี, แรงงัดให้ลอยขึ้น, ชนิดของแรง)
            rb.AddExplosionForce(pushForce * 2f, pointUnderChair, 2f, 1.5f, ForceMode.Impulse);

            // 🌟 เพิ่มแรงบิดให้เก้าอี้หมุนตีลังกาหงายหลังชัวร์ๆ
            rb.AddTorque(transform.right * pushForce, ForceMode.Impulse);
        }

        if (fallSound != null)
        {
            fallSound.Play();
        }

        Debug.Log("ปัง!! ผีใช้พลังระเบิดงัดเก้าอี้ล้มตีลังกา!!");
    }
}