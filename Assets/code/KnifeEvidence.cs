using UnityEngine; 
// เรียกใช้ไลบรารีหลักของ Unity สำหรับการทำงานทั่วไป เช่น GameObject, Input, Collider

public class KnifeEvidence : MonoBehaviour
// สร้างคลาสชื่อ KnifeEvidence และสืบทอดจาก MonoBehaviour
// เพื่อให้สามารถนำไปใส่ใน GameObject ใน Unity ได้
{
    [Header("ตัวมีดในฉาก")]
    // แค่เอาไว้จัดหมวดใน Inspector ให้อ่านง่ายขึ้น

    public GameObject knife3DModel;
    // ตัวแปรเก็บ "โมเดลมีดในฉากจริง" (3D)
    // เอาไว้เปิด/ปิดการมองเห็นของมีด

    [Header("ช่องหลักฐานในกระเป๋า")]
    // หัวข้อใน Inspector

    public GameObject evidenceUI;
    // UI ที่เป็นช่องเก็บของ (inventory) ของมีด
    // ตอนแรกจะปิดไว้ พอเก็บมีดจะเปิด

    private bool canPickup = false;
    // ตัวแปรตรวจสอบว่า "ผู้เล่นอยู่ใกล้มีดพอจะเก็บได้ไหม"
    // เริ่มต้นเป็น false (ยังเก็บไม่ได้)

    void Update()
    // ฟังก์ชันนี้จะทำงานทุกเฟรม
    {
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        // ถ้าอยู่ในระยะเก็บ (canPickup = true)
        // และผู้เล่นกดปุ่ม F
        {
            PickUpKnife();
            // เรียกฟังก์ชันเก็บมีด
        }
    }

    private void OnTriggerEnter(Collider other)
    // ฟังก์ชันจะทำงานเมื่อมีวัตถุเข้ามาชน Collider (แบบ Trigger)
    {
        if (other.CompareTag("Player"))
        // เช็คว่าวัตถุที่ชนคือ Player หรือไม่
        {
            canPickup = true;
            // ถ้าใช่ → ให้สามารถเก็บมีดได้
        }
    }

    private void OnTriggerExit(Collider other)
    // ฟังก์ชันจะทำงานเมื่อวัตถุออกจาก Collider
    {
        if (other.CompareTag("Player"))
        // ถ้าเป็น Player
        {
            canPickup = false;
            // ออกจากระยะแล้ว → เก็บไม่ได้
        }
    }

    void PickUpKnife()
    // ฟังก์ชันสำหรับ "เก็บมีด"
    {
        // เปิดช่องหลักฐาน
        evidenceUI.SetActive(true);
        // แสดง UI ช่องเก็บของของมีด

        // ซ่อนมีดในฉาก
        knife3DModel.SetActive(false);
        // ปิดการแสดงผลมีดในโลกจริง (เหมือนเก็บไปแล้ว)

        canPickup = false;
        // ปิดการเก็บซ้ำ
    }
}