using System.Collections;
using UnityEngine;
using TMPro; // 🌟 ต้องมีบรรทัดนี้เพื่อคุม TextMeshPro

public class NotificationManager : MonoBehaviour
{
    // 🌟 บรรทัดนี้ทำให้เราเรียกใช้สคริปต์นี้จากที่ไหนก็ได้ในเกม!
    public static NotificationManager instance;

    [Header("ช่องใส่ UI ข้อความ (ลาก NotificationText มาใส่)")]
    public TextMeshProUGUI notificationText;

    [Header("เวลาที่โชว์ข้อความ (วินาที)")]
    public float showTime = 2.5f;

    void Awake()
    {
        instance = this; // ตั้งค่าตัวมันเองให้เป็นศูนย์กลาง

        if (notificationText != null)
        {
            notificationText.text = ""; // เริ่มเกมมา ให้เคลียร์ข้อความทิ้งไปก่อน
        }
    }

    // 🌟 ฟังก์ชันรับออเดอร์: ใครอยากโชว์ข้อความอะไร ให้ส่งมาที่นี่!
    public void ShowText(string message)
    {
        StopAllCoroutines(); // หยุดการนับเวลาอันเก่า (เผื่อผู้เล่นเก็บของรัวๆ)
        StartCoroutine(ShowAndHide(message));
    }

    // 🌟 ระบบโชว์แล้วซ่อน
    IEnumerator ShowAndHide(string message)
    {
        notificationText.text = message; // เปลี่ยนข้อความเป็นสิ่งที่เก็บได้
        yield return new WaitForSeconds(showTime); // รอเวลา 2.5 วินาที
        notificationText.text = ""; // ลบข้อความทิ้ง
    }
}