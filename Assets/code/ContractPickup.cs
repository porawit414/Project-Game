using UnityEngine;

public class ContractPickup : MonoBehaviour
{
    // 🌟 1. เพิ่มช่องตั้งชื่อไอเทมสำหรับโชว์แจ้งเตือน
    [Header("ชื่อไอเทมที่จะโชว์ตอนแจ้งเตือน")]
    public string itemName = "ใบสัญญากู้ยืม";

    [Header("แผ่นสัญญาในฉาก")]
    public GameObject contract3DModel;

    [Header("ช่องสัญญาในกระเป๋า (UI)")]
    public GameObject contractUI;

    [Header("ระบบเสียงตอนเก็บ")]
    public AudioClip pickupSound; // ลากเสียงหยิบกระดาษ (Paper rustle) มาใส่

    private bool canPickup = false;

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

        Debug.Log("เก็บหลักฐานใบสัญญาแล้ว!");
    }
}