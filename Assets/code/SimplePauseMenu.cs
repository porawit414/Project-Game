using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SimplePauseMenu : MonoBehaviour
{
    [Header("หน้าต่างเมนู (ลาก Pause Panel มาใส่)")]
    public GameObject pauseMenuPanel;

    // 🌟 1. เพิ่มช่องให้ลากหน้าต่างตั้งค่ามาใส่ 🌟
    [Header("หน้าต่างตั้งค่า (ลาก Settings Panel มาใส่)")]
    public GameObject settingsMenuPanel;

    [Header("สคริปต์กระเป๋า (ป้องกันเมนูตีกัน)")]
    public InventoryUIController inventoryController;

    [Header("ตัวละคร (ลาก PlayerCapsule มาใส่)")]
    public GameObject playerObject;

    [Header("🌟 อนุญาตให้กด ESC หยุดเกมได้หรือไม่")]
    public bool canPause = true;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false); // ปิดหน้าตั้งค่าไว้ก่อนตอนเริ่ม

        isPaused = false;
        canPause = true;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // ถ้าระบบกระเป๋าเปิดอยู่ จะข้ามคำสั่ง Pause ไปเลย ให้กระเป๋าทำงานแทน
        if (inventoryController != null && inventoryController.isInventoryOpen)
        {
            return;
        }

        // ระบบ Pause ปกติ ทำงานก็ต่อเมื่อกระเป๋าปิดอยู่เท่านั้น
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            // 🌟 เช็คว่าถ้าเปิด "หน้าตั้งค่า" ค้างไว้อยู่ ให้กดย้อนกลับมาที่หน้า Pause หลัก
            if (settingsMenuPanel != null && settingsMenuPanel.activeSelf)
            {
                CloseSettings();
            }
            // ถ้าหน้า Pause หลักเปิดอยู่ ให้กลับเข้าเกม
            else if (isPaused)
            {
                ResumeGame();
            }
            // ถ้าไม่มีอะไรเปิดอยู่เลย ให้หยุดเกม
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        TogglePlayerInput(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false); // ปิดหน้าตั้งค่าด้วยเพื่อความชัวร์

        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        TogglePlayerInput(true);
    }

    // ----------------------------------------------------------------
    // 🌟 ฟังก์ชันใหม่: เอาไว้ผูกกับปุ่มต่างๆ 🌟

    public void OpenSettings()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false); // ซ่อนหน้า Pause หลัก
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(true); // โชว์หน้าตั้งค่า
    }

    public void CloseSettings()
    {
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false); // ซ่อนหน้าตั้งค่า
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true); // โชว์หน้า Pause หลักกลับมา
    }
    // ----------------------------------------------------------------

    public void GoToMainMenu()
    {
        Debug.Log("🏠 กำลังกลับไปหน้าเมนูหลัก...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMen");
    }

    public void TogglePlayerInput(bool state)
    {
        if (playerObject != null)
        {
            Behaviour playerInput = playerObject.GetComponent("UnityEngine.InputSystem.PlayerInput") as Behaviour;
            if (playerInput != null) playerInput.enabled = state;

            MonoBehaviour fpsController = playerObject.GetComponent("StarterAssets.FirstPersonController") as MonoBehaviour;
            if (fpsController != null) fpsController.enabled = state;
        }
    }

    public void QuitGame()
    {
        Debug.Log("ออกจากเกม!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}