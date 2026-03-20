using UnityEngine;
using TMPro; // สำคัญมากสำหรับการควบคุม TMP Text

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject tutorialNote; // ลาก Object กระดาษคำอธิบายของคุณมาใส่ที่นี่
    public TextMeshProUGUI promptText; // ลาก Object ข้อความ "กด 1" มาใส่ที่นี่

    [Header("Player Control")]
    public MonoBehaviour playerMovement; // ลากสคริปต์ควบคุมการเดินของตัวละครมาใส่ (เช่น FirstPersonController)

    private bool isNoteOpen = false;

    void Start()
    {
        // ตอนเริ่มเกม ให้ซ่อนกระดาษคำอธิบายไว้ก่อน
        if (tutorialNote != null) tutorialNote.SetActive(false);
        // และแสดงข้อความบอกวิธีเปิด
        if (promptText != null) promptText.enabled = true;
    }

    void Update()
    {
        // เช็คว่าผู้เล่นกดปุ่ม 1 หรือไม่ (GetKeyDown เพื่อให้กดครั้งเดียว)
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            ToggleTutorialNote();
        }
    }

    void ToggleTutorialNote()
    {
        if (tutorialNote == null) return;

        isNoteOpen = !isNoteOpen; // สลับสถานะ

        if (isNoteOpen)
        {
            // --- เปิดกระดาษ ---
            tutorialNote.SetActive(true);
            // เปลี่ยนข้อความ prompt หรือจะซ่อนไปเลยก็ได้
            if (promptText != null) promptText.text = "กด 1 เพื่อปิดคำอธิบาย";

            // ล็อคการเคลื่อนที่ของตัวละครเพื่อให้เน้นอ่าน
            if (playerMovement != null) playerMovement.enabled = false;

            // ปลดล็อคเมาส์ (ถ้าต้องใช้เมาส์กดปิดในหน้ากระดาษ)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // --- ปิดกระดาษ ---
            tutorialNote.SetActive(false);
            // เปลี่ยนข้อความ prompt กลับ
            if (promptText != null) promptText.text = "กด 1 เพื่อเปิดคำอธิบาย";

            // ปลดล็อคให้ตัวละครเดินได้ปกติ
            if (playerMovement != null) playerMovement.enabled = true;

            // ล็อคเมาส์กลับเข้าเกม
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}