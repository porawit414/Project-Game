using UnityEngine;         // ใช้สำหรับเข้าถึงระบบหลักของ Unity เช่น GameObject, MonoBehaviour
using UnityEngine.UI;      // ใช้สำหรับทำงานกับ UI เช่น Button, Image, Panel

public class KnifeButton : MonoBehaviour   // สร้าง Script ชื่อ KnifeButton และให้ใช้งานกับ GameObject ได้
{
    public GameObject knifePanel; // ตัวแปรเก็บ Panel UI ที่จะใช้แสดง "ข้อมูลมีด"

    // ฟังก์ชันนี้จะถูกเรียกตอน "กดปุ่มมีด"
    public void OnClickKnife()
    {
        // เช็คก่อนว่าเราได้ลาก Panel มาใส่ใน Inspector หรือยัง
        if (knifePanel != null)
        {
            // ถ้ามี Panel -> ให้เปิด (แสดงขึ้นมา)
            knifePanel.SetActive(true); 
        }
        else
        {
            // ถ้ายังไม่ได้ใส่ Panel -> แสดง error ใน Console
            Debug.Log("Knife Panel not assigned!");
        }
    }

    // ฟังก์ชันนี้ใช้สำหรับ "ปิดหน้าข้อมูลมีด"
    public void CloseKnifePanel()
    {
        // เช็คว่ามี Panel หรือไม่
        if (knifePanel != null)
        {
            // ถ้ามี -> ปิด Panel (ซ่อน)
            knifePanel.SetActive(false);
        }
    }
}