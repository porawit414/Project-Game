using UnityEngine;

public class BloodyShirtPickup : MonoBehaviour
{
    [Header("ตัวเสื้อในฉาก")]
    public GameObject shirt3DModel;

    [Header("ช่องในกระเป๋า")]
    public GameObject shirtUI;

    [Header("จุดดักเสียงเคาะประตู (ส่วนที่เพิ่มใหม่)")]
    public GameObject doorKnockTrigger; // ลากกล่อง DoorKnock ล่องหน มาใส่ช่องนี้

    [Header("ระบบเสียงตอนเก็บเสื้อ")]
    public AudioClip pickupSound; // ลากไฟล์เสียงหยิบผ้า/เสื้อ มาใส่ช่องนี้

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
        // === จุดที่เพิ่ม: สั่งให้ตัวนับหลักฐานทำงาน (+1) ===
        if (GameManager.instance != null)
        {
            GameManager.instance.AddEvidence();
        }

        // 🌟 0. สั่งเล่นเสียงหยิบเสื้อตรงนี้!
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
            doorKnockTrigger.SetActive(true); // เปิดกล่องดักให้เริ่มทำงาน
        }

        canPickup = false;

        // 4. ปิดกล่อง Trigger ของเสื้อทิ้ง
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log("เก็บหลักฐานเสื้อเปื้อนเลือดแล้ว!");
    }
}