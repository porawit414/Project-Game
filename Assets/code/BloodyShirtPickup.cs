using UnityEngine;

public class BloodyShirtPickup : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของเสื้อเปื้อนเลือด (ห้ามซ้ำ)")]
    public string shirtSaveKey = "Evidence_BloodyShirt";

    [Header("ชื่อไอเทมที่จะโชว์ตอนแจ้งเตือน")]
    public string itemName = "เสื้อเปื้อนเลือด";

    [Header("ตัวเสื้อในฉาก")]
    public GameObject shirt3DModel;

    [Header("ช่องในกระเป๋า")]
    public GameObject shirtUI;

    [Header("จุดดักเสียงเคาะประตู (ส่วนที่เพิ่มใหม่)")]
    public GameObject doorKnockTrigger;

    [Header("ระบบเสียงตอนเก็บเสื้อ")]
    public AudioClip pickupSound;

    private bool canPickup = false;

    void Start()
    {
        // 🌟 1. เช็คตอนเริ่มเกมว่า "เคยเก็บเสื้อเล่มนี้ไปหรือยัง?"
        // ถ้า PlayerPrefs มีค่าเป็น 1 แปลว่าเคยเก็บแล้ว
        if (PlayerPrefs.GetInt(shirtSaveKey, 0) == 1)
        {
            // เปิดช่องเสื้อในกระเป๋ารอไว้เลย
            if (shirtUI != null) shirtUI.SetActive(true);

            // ซ่อนเสื้อในฉากทิ้งไป
            if (shirt3DModel != null) shirt3DModel.SetActive(false);

            // หมายเหตุ: เราไม่เปิด doorKnockTrigger ซ้ำนะ ให้ผีหลอกแค่รอบแรกพอ 👻

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
            PickUpShirt();
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

    void PickUpShirt()
    {
        // === สั่งให้ตัวนับหลักฐานทำงาน (+1) ===
        if (GameManager.instance != null)
        {
            GameManager.instance.AddEvidence();
        }

        // 🌟 2. สั่งโชว์ข้อความแจ้งเตือนตรงนี้!
        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowText("ได้รับ: " + itemName);
        }

        // 0. สั่งเล่นเสียงหยิบเสื้อ
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // 1. เปิดช่องในกระเป๋า
        if (shirtUI != null) shirtUI.SetActive(true);

        // 2. ซ่อนเสื้อในฉาก
        if (shirt3DModel != null) shirt3DModel.SetActive(false);

        // 3. --- สั่งให้กล่องดักเสียงเคาะประตูทำงาน! --- (ผีหลอกทำงาน!)
        if (doorKnockTrigger != null)
        {
            doorKnockTrigger.SetActive(true);
        }

        canPickup = false;

        // 4. ปิดกล่อง Trigger ของเสื้อทิ้ง
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 🌟 3. เซฟลงเครื่องว่า "เก็บเสื้อเปื้อนเลือดไปแล้ว! (ค่า = 1)"
        PlayerPrefs.SetInt(shirtSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บหลักฐานเสื้อเปื้อนเลือดแล้ว!");

        // ปิดการทำงานสคริปต์
        this.enabled = false;
    }
}