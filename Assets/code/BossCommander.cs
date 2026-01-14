using UnityEngine;
using UnityEngine.UI;
using TMPro; // จำเป็นต้องมีบรรทัดนี้เพื่อใช้ TextMeshPro

public class BossCommander : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject dialoguePanel;      // ตัวกล่องข้อความทั้งหมด
    public Image bossFace;                // รูปหัวหน้า
    public TextMeshProUGUI commandText;   // ที่แสดงข้อความ

    [Header("Settings")]
    public Sprite bossImageSource;        // รูปไฟล์ที่จะเอามาใส่
    [TextArea(3, 5)]                      // ทำให้ช่องกรอกข้อความกว้างขึ้น
    public string startMessage = "ทหาร! ภารกิจของคุณคือบุกไปยึดพื้นที่ด้านหน้า ระวังตัวด้วย!";

    void Start()
    {
        // สั่งให้ทำงานทันทีเมื่อเริ่มเกม
        ShowCommand(startMessage);
    }

    public void ShowCommand(string message)
    {
        // 1. เปิดหน้าต่าง (เผื่อมันปิดอยู่)
        dialoguePanel.SetActive(true);

        // 2. ใส่รูปหัวหน้า (ถ้ามี)
        if (bossImageSource != null)
        {
            bossFace.sprite = bossImageSource;
        }

        // 3. ใส่ข้อความ
        commandText.text = message;
    }

    // ฟังก์ชันสำหรับปิดหน้าต่าง (เอาไว้เรียกใช้ทีหลัง เช่น กดปุ่มปิด)
    public void CloseCommand()
    {
        dialoguePanel.SetActive(false);
    }
}