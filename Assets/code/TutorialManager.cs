using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    // 🚨 1. สร้างป้ายไฟเตือนให้สคริปต์อื่นรู้ว่า "หน้าต่างสอนเล่นเปิดอยู่!"
    public static bool isTutorialOpen = false;

    [Header("UI Elements")]
    public GameObject tutorialNote;
    public TextMeshProUGUI promptText;

    [Header("Player Control")]
    public MonoBehaviour playerMovement;

    private bool isNoteOpen = false;
    private bool isSystemDisabled = false;

    void Start()
    {
        isTutorialOpen = false; // รีเซ็ตตอนเริ่มเกม
        if (tutorialNote != null) tutorialNote.SetActive(false);
        if (promptText != null)
        {
            promptText.enabled = true;
            promptText.text = "กด [Space Bar] เพื่อเปิดคำอธิบาย";
        }
    }

    void Update()
    {
        if (isSystemDisabled) return;

        // เช็คไม้กั้นจากหัวหน้าและหน้าต่างเมนู Pause
        if (IntroDialog.isIntroActive || SimplePauseMenu.isGamePaused)
        {
            if (promptText != null) promptText.enabled = false;
            return;
        }
        else
        {
            if (promptText != null && !promptText.enabled)
            {
                promptText.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleTutorialNote();
        }
    }

    public void DisableTutorialSystem()
    {
        isSystemDisabled = true;
        isTutorialOpen = false; // ปิดป้ายไฟด้วย
        if (tutorialNote != null) tutorialNote.SetActive(false);
        if (promptText != null) promptText.gameObject.SetActive(false);
        this.enabled = false;
    }

    void ToggleTutorialNote()
    {
        if (tutorialNote == null) return;

        isNoteOpen = !isNoteOpen;
        isTutorialOpen = isNoteOpen; // 🌟 2. อัปเดตสถานะป้ายไฟเตือน!

        if (isNoteOpen)
        {
            tutorialNote.SetActive(true);
            if (promptText != null) promptText.text = "กด [Space Bar] เพื่อปิดคำอธิบาย";

            // สั่งล็อคไม่ให้เดิน (ถ้าลากสคริปต์เดินมาใส่ใน Inspector แล้ว มันจะหยุดเดินทันที)
            if (playerMovement != null) playerMovement.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            tutorialNote.SetActive(false);
            if (promptText != null) promptText.text = "กด [Space Bar] เพื่อเปิดคำอธิบาย";

            // สั่งให้กลับมาเดินได้ปกติ
            if (playerMovement != null) playerMovement.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}