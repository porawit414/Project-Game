using UnityEngine;

public class DiaryObject : MonoBehaviour
{
    public DiaryData myData; // ลากไฟล์ DiaryData ที่สร้างในข้อ 1 มาใส่ช่องนี้ใน Inspector

    public void Interact()
    {
        // ส่งข้อมูลเข้ากระเป๋า
        InventoryManager.Instance.AddDiary(myData);

        // ลบของชิ้นนี้ออกจากฉาก (เก็บแล้วหายไป)
        Destroy(gameObject);
    }
}