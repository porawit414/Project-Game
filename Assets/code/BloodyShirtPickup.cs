using UnityEngine;

public class BloodyShirtPickup : MonoBehaviour
{
    [Header("ตัวเสื้อในฉาก")]
    public GameObject shirt3DModel;

    [Header("ช่องในกระเป๋า")]
    public GameObject shirtUI;

    [Header("จุดดักเสียงเคาะประตู (ส่วนที่เพิ่มใหม่)")]
    public GameObject doorKnockTrigger; // ลากกล่อง DoorKnock ล่องหน มาใส่ช่องนี้

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
        
        // 4. (แถม) ปิดกล่อง Trigger ของเสื้อทิ้งไปเลย ผู้เล่นจะได้ไม่มากด F ซ้ำได้อีก
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }
}