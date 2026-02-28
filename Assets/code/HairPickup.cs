using UnityEngine;

public class HairPickup : MonoBehaviour
{
    // 🌟 1. เพิ่มช่องตั้งชื่อไอเทมสำหรับโชว์แจ้งเตือน
    [Header("ชื่อไอเทมที่จะโชว์ตอนแจ้งเตือน")]
    public string itemName = "เส้นผมปริศนา";

    [Header("ตัวโมเดลผมในฉาก")]
    public GameObject hair3DModel;

    [Header("ช่องหลักฐานผมในกระเป๋า")]
    public GameObject hairUI;

    [Header("ระบบเสียงตอนเก็บ")]
    public AudioClip pickupSound; // หาเสียงหยิบของมาใส่

    private bool canPickup = false;

    void Update()
    {
        // ถ้าผู้เล่นอยู่ใกล้และกดปุ่ม F
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            PickUpHair();
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

    void PickUpHair()
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

        // 1. เล่นเสียงตอนเก็บ
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // 2. เปิดโชว์รูปผมในกระเป๋า (UI)
        if (hairUI != null) hairUI.SetActive(true);

        // 3. ซ่อนโมเดลผมในฉาก
        if (hair3DModel != null) hair3DModel.SetActive(false);

        canPickup = false;

        // 4. ปิดกล่องชน จะได้ไม่เผลอมากดเก็บซ้ำ
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log("เก็บหลักฐานเส้นผมแล้ว!");
    }
}