using UnityEngine;
using TMPro; // สำคัญมากสำหรับการควบคุม TMP Text

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject tutorialNote; // ลาก Object กระดาษคำอธิบายของคุณมาใส่ที่นี่
    public TextMeshProUGUI promptText; // ลาก Object ข้อความ "กด [Space Bar]" มาใส่ที่นี่

    [Header("Player Control")]
    public MonoBehaviour playerMovement; // ลากสคริปต์ควบคุมการเดินมาใส่

    private bool isNoteOpen = false;
    private bool isSystemDisabled = false; // ตัวแปรเช็คว่าโดนสั่งปิดระบบหรือยัง (ตอนจบเกม)

    void Start()
    {
        // ตอนเริ่มเกม ให้ซ่อนกระดาษคำอธิบายไว้ก่อน
        if (tutorialNote != null) tutorialNote.SetActive(false);
        // และแสดงข้อความบอกวิธีเปิด
        if (promptText != null) 
        {
            promptText.enabled = true;
            promptText.text = "กด [Space Bar] เพื่อเปิดคำอธิบาย"; // ตั้งค่าเริ่มต้น
        }
    }

    void Update()
    {
        // ถ้าโดนสั่งปิดระบบ (จากสคริปต์ประตูฉากจบ) ไม่ต้องทำงานต่อ
        if (isSystemDisabled) return;

        // เปลี่ยนเป็นเช็คปุ่ม Space Bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleTutorialNote();
        }
    }

    // ฟังก์ชันสั่งปิดระบบสอนทั้งหมด (เรียกใช้จากสคริปต์ประตูฉากจบ)
    public void DisableTutorialSystem()
    {
        isSystemDisabled = true;
        if (tutorialNote != null) tutorialNote.SetActive(false);
        if (promptText != null) promptText.gameObject.SetActive(false); 
        this.enabled = false; 
    }

    void ToggleTutorialNote()
    {
        if (tutorialNote == null) return;

        isNoteOpen = !isNoteOpen; // สลับสถานะ

        if (isNoteOpen)
        {
            // --- เปิดกระดาษ ---
            tutorialNote.SetActive(true);
            if (promptText != null) promptText.text = "กด [Space Bar] เพื่อปิดคำอธิบาย";

            // ล็อคการเคลื่อนที่ของตัวละคร
            if (playerMovement != null) playerMovement.enabled = false;

            // ปลดล็อคเมาส์
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // --- ปิดกระดาษ ---
            tutorialNote.SetActive(false);
            if (promptText != null) promptText.text = "กด [Space Bar] เพื่อเปิดคำอธิบาย";

            // ปลดล็อคให้ตัวละครเดินได้ปกติ
            if (playerMovement != null) playerMovement.enabled = true;

            // ล็อคเมาส์กลับเข้าเกม
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}