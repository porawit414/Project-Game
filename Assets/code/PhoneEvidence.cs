using UnityEngine;
using UnityEngine.Events;

public class PhoneEvidence : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของโทรศัพท์มือถือ (ห้ามซ้ำ)")]
    public string phoneSaveKey = "Evidence_Phone";

    [Header("ข้อมูลหลักฐาน")]
    public string itemName = "โทรศัพท์มือถือปริศนา";
    public GameObject phone3DModel;
    public GameObject phoneUI;
    public AudioClip pickupSound;

    [Header("เหตุการณ์หลอนทิ้งท้าย")]
    public UnityEvent onPhonePickedUp;

    private bool canPickup = false;

    void Start()
    {
        // 🌟 1. เช็คตอนเริ่มเกมว่า "เคยเก็บโทรศัพท์เครื่องนี้ไปหรือยัง?"
        if (PlayerPrefs.GetInt(phoneSaveKey, 0) == 1)
        {
            // เปิดปุ่ม UI มือถือในกระเป๋ารอไว้เลย
            if (phoneUI != null) phoneUI.SetActive(true);

            // ซ่อนโมเดลในฉาก
            if (phone3DModel != null)
            {
                phone3DModel.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }

            // หมายเหตุ: ไม่สั่งรัน UnityEvent ซ้ำนะ ให้ผีหลอกแค่รอบแรกพอ 👻

            // ปิดกล่องชนและสคริปต์นี้ทิ้งไปเลย
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
            this.enabled = false;
        }
    }

    void Update()
    {
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
        // === สั่งให้ตัวนับหลักฐานทำงาน ===
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
            gameObject.SetActive(false);
        }

        canPickup = false;

        // 5. ปิดกล่อง Collider เพื่อกันกดซ้ำ
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 🌟 2. เซฟลงเครื่องว่า "เก็บโทรศัพท์มือถือไปแล้ว! (ค่า = 1)"
        PlayerPrefs.SetInt(phoneSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บหลักฐานชิ้นสุดท้ายสำเร็จ: " + itemName);

        // ปิดการทำงานสคริปต์
        this.enabled = false;
    }
}