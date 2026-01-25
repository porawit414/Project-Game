using UnityEngine;
using TMPro; // เพิ่มบรรทัดนี้เพื่อใช้ TextMeshPro
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey = false;

    [Header("UI สำหรับแจ้งเตือน")]
    public TextMeshProUGUI notificationText; // ลาก TextMeshPro ที่สร้างมาใส่ช่องนี้

    // ฟังก์ชันเรียกใช้แสดงข้อความ (เรียกจาก Script กุญแจ)
    public void ShowNotification(string message)
    {
        StartCoroutine(ShowTextTimer(message));
    }

    // ตัวนับเวลา ให้ข้อความขึ้น 3 วินาทีแล้วหายไป
    IEnumerator ShowTextTimer(string message)
    {
        notificationText.text = message;      // เปลี่ยนข้อความ
        notificationText.gameObject.SetActive(true); // เปิดการมองเห็น

        yield return new WaitForSeconds(3);   // รอ 3 วินาที

        notificationText.gameObject.SetActive(false); // ซ่อนข้อความ
    }
}