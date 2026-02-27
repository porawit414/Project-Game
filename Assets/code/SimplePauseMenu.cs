using UnityEngine;
using UnityEngine.EventSystems;

public class SimplePauseMenu : MonoBehaviour
{
    [Header("หน้าต่างเมนู (ลาก Pause Panel มาใส่)")]
    public GameObject pauseMenuPanel;

    [Header("ตัวละคร (ลาก PlayerCapsule มาใส่)")]
    public GameObject playerObject;

    private bool isPaused = false;

    void Start()
    {
        // เริ่มเกมมา ปิดเมนูและให้เวลาเดินปกติ
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // กด ESC เพื่อสลับโหมดเปิด/ปิดเมนู
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // หยุดเวลาในเกม

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true); // เปิดหน้าต่างเมนู

        // ปลดล็อคเมาส์ให้ขยับมากดปุ่มได้
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // สั่งระงับการควบคุมตัวละคร (ตัดไฟคีย์บอร์ดและเมาส์)
        TogglePlayerInput(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // ให้เวลาเดินปกติ

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false); // ปิดหน้าต่างเมนู

        // เคลียร์ความจำปุ่ม (กันบัคเมาส์จำปุ่มค้างตอนกด ESC)
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

        // ล็อคเมาส์กลับเข้ากลางจอเพื่อเล่นเกมต่อ
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // คืนการควบคุมให้ตัวละคร
        TogglePlayerInput(true);
    }

    // ฟังก์ชันพิเศษ: ตัดไฟ/จ่ายไฟ ให้ระบบควบคุมของ Starter Assets โดยเฉพาะ
    private void TogglePlayerInput(bool state)
    {
        if (playerObject != null)
        {
            // ปิดตัวรับ Input (ทำให้ขยับเมาส์/เดินไม่ได้ 100%)
            Behaviour playerInput = playerObject.GetComponent("UnityEngine.InputSystem.PlayerInput") as Behaviour;
            if (playerInput != null) playerInput.enabled = state;

            // ปิดตัวควบคุมการเดิน
            MonoBehaviour fpsController = playerObject.GetComponent("StarterAssets.FirstPersonController") as MonoBehaviour;
            if (fpsController != null) fpsController.enabled = state;
        }
    }

    public void QuitGame()
    {
        Debug.Log("ออกจากเกม!");
        Application.Quit();
    }
}