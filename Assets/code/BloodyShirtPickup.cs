using UnityEngine;

public class BloodyShirtPickup : MonoBehaviour
{
    // 🌟 1. เพิ่มช่องให้ตั้งชื่อได้เหมือนสมุดไดอารี่
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

        // 3. --- สั่งให้กล่องดักเสียงเคาะประตูทำงาน! ---
        if (doorKnockTrigger != null)
        {
            doorKnockTrigger.SetActive(true);
        }

        canPickup = false;

        // 4. ปิดกล่อง Trigger ของเสื้อทิ้ง
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log("เก็บหลักฐานเสื้อเปื้อนเลือดแล้ว!");
    }
}