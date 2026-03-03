using UnityEngine;

public class ContractPickup : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของใบสัญญา (ห้ามซ้ำ)")]
    public string contractSaveKey = "Evidence_Contract";

    [Header("ชื่อไอเทมที่จะโชว์ตอนแจ้งเตือน")]
    public string itemName = "ใบสัญญากู้ยืม";

    [Header("แผ่นสัญญาในฉาก")]
    public GameObject contract3DModel;

    [Header("ช่องสัญญาในกระเป๋า (UI)")]
    public GameObject contractUI;

    [Header("ระบบเสียงตอนเก็บ")]
    public AudioClip pickupSound;

    private bool canPickup = false;

    void Start()
    {
        // 🌟 1. เช็คตอนเริ่มเกมว่า "เคยเก็บใบสัญญาไปหรือยัง?"
        // ถ้า PlayerPrefs มีค่าเป็น 1 แปลว่าเคยเก็บแล้ว
        if (PlayerPrefs.GetInt(contractSaveKey, 0) == 1)
        {
            // โชว์รูปสัญญาในกระเป๋าให้เลย
            if (contractUI != null) contractUI.SetActive(true);

            // ซ่อนแผ่นสัญญาในฉาก
            if (contract3DModel != null) contract3DModel.SetActive(false);

            // ปิดกล่องชนและสคริปต์ทิ้ง
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
            this.enabled = false;
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            PickUpContract();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) canPickup = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) canPickup = false;
    }

    void PickUpContract()
    {
        // === จุดที่เพิ่ม: สั่งให้ตัวนับหลักฐานทำงาน (+1) ===
        if (GameManager.instance != null)
        {
            GameManager.instance.AddEvidence();
        }

        // 🌟 2. สั่งโชว์ข้อความแจ้งเตือนตรงนี้! 🌟
        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowText("ได้รับ: " + itemName);
        }

        // 1. เล่นเสียงหยิบกระดาษ
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // 2. โชว์รูปสัญญาในกระเป๋า
        if (contractUI != null) contractUI.SetActive(true);

        // 3. ซ่อนแผ่นสัญญาในฉาก
        if (contract3DModel != null) contract3DModel.SetActive(false);

        canPickup = false;

        // 4. ปิดกล่องชน
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 🌟 3. เซฟลงเครื่องว่า "เก็บใบสัญญาไปแล้ว! (ค่า = 1)"
        PlayerPrefs.SetInt(contractSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บหลักฐานใบสัญญาแล้ว!");

        // ปิดการทำงานสคริปต์
        this.enabled = false;
    }
}