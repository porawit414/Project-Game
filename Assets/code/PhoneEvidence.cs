using UnityEngine;
using UnityEngine.Events;

public class PhoneEvidence : MonoBehaviour
{
    [Header("ข้อมูลหลักฐาน")]
    public string itemName = "โทรศัพท์มือถือปริศนา";
    public GameObject phone3DModel; // ลากโมเดลมือถือในฉากมาใส่
    public GameObject phoneUI;      // ลาก UI มือถือในกระเป๋ามาใส่
    public AudioClip pickupSound;   // เสียงตอนเก็บ

    [Header("เหตุการณ์หลอนทิ้งท้าย")]
    public UnityEvent onPhonePickedUp;

    private bool canPickup = false;

    void Update()
    {
        // ระบบกด F เมื่ออยู่ใกล้เหมือนสคริปต์เสื้อ
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            PickUpPhone();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }

    public void PickUpPhone()
    {
        // === จุดที่เพิ่ม: สั่งให้ตัวนับหลักฐานทำงาน ===
        if (GameManager.instance != null)
        {
            GameManager.instance.AddEvidence();
        }

        // 🌟 0. แจ้งเตือนบนหน้าจอว่าเก็บของแล้ว! 🌟
        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowText("ได้รับ: " + itemName);
        }

        // 1. เล่นเสียงเก็บ
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // 2. เปิด UI มือถือในกระเป๋า
        if (phoneUI != null)
        {
            phoneUI.SetActive(true);
        }

        // 3. สั่งงาน Event หลอนๆ (ถ้ามีลากอะไรมาใส่ไว้)
        onPhonePickedUp.Invoke();

        // 4. ซ่อนโมเดลในฉาก
        if (phone3DModel != null)
        {
            phone3DModel.SetActive(false);
        }
        else
        {
            // ถ้าไม่ได้ลากใส่ช่อง phone3DModel ให้หายไปทั้ง Object ที่แปะสคริปต์เลย
            gameObject.SetActive(false);
        }

        canPickup = false;

        // 5. ปิดกล่อง Collider เพื่อกันกดซ้ำ
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log("เก็บหลักฐานชิ้นสุดท้ายสำเร็จ: " + itemName);
    }
}