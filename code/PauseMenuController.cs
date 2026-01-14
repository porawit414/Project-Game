using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel; // ลาก Panel เมนูหลักมาใส่
    public GameObject settingsPanel;  // ลาก Panel ตั้งค่ามาใส่ (ถ้ามี)

    public static bool isPaused = false;

    void Start()
    {
        // เริ่มเกมมาให้ปิดเมนูไปก่อน
        pauseMenuPanel.SetActive(false);
        if(settingsPanel != null) settingsPanel.SetActive(false);
    }

    void Update()
    {
        // กด ESC เพื่อเปิด/ปิด เมนู
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // หยุดเวลาในเกม
        isPaused = true;
        
        // ปลดล็อคเมาส์ให้กดปุ่มได้ (สำคัญมากสำหรับเกม FPS)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        if(settingsPanel != null) settingsPanel.SetActive(false);
        
        Time.timeScale = 1f; // ให้เวลาเดินต่อ
        isPaused = false;

        // ล็อคเมาส์กลับไปเล็งปืน/มองรอบๆ (สำหรับเกม FPS)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        // ปิดหน้าเมนูหลัก เปิดหน้าตั้งค่า
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        // ปิดหน้าตั้งค่า กลับมาหน้าเมนูหลัก
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game..."); // เช็คใน Unity Editor
        Application.Quit(); // ทำงานจริงตอน Build เกมแล้ว
    }
}