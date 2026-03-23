using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SimplePauseMenu : MonoBehaviour
{
    // 🚨 ป้ายไฟเตือน: บอกสคริปต์อื่นว่าตอนนี้เปิดเมนู Pause อยู่ไหม
    public static bool isGamePaused = false;

    [Header("หน้าต่างเมนู (ลาก Pause Panel มาใส่)")]
    public GameObject pauseMenuPanel;

    [Header("หน้าต่างตั้งค่า (ลาก Settings Panel มาใส่)")]
    public GameObject settingsMenuPanel;

    [Header("หน้าต่างคำสั่งหัวหน้า (เอาไว้ล็อคปุ่ม ESC)")]
    public GameObject bossUI;

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
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);

        isPaused = false;
        isGamePaused = false; // 🌟 รีเซ็ตป้ายไฟตอนเริ่มเกม
        canPause = true;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // 🚨 ไม้กั้นใหม่ 1: เช็คว่าหน้ากระดาษสอนเล่นเปิดอยู่ไหม? ถ้าเปิดอยู่ ห้ามกด ESC เด็ดขาด!
        if (TutorialManager.isTutorialOpen)
        {
            return;
        }

        // 🚨 ไม้กั้นใหม่ 2: เช็คว่าสคริปต์ IntroDialog เปิดกระดาษคำสั่งหัวหน้าอยู่ไหม? (เช็คจากป้ายไฟ)
        // (บรรทัดนี้ช่วยป้องกันบั๊กตอนโหลดฉากได้ดีกว่าเช็ค activeInHierarchy ธรรมดาครับ)
        if (IntroDialog.isIntroActive)
        {
            return;
        }

        if (inventoryController != null && inventoryController.isInventoryOpen)
        {
            return;
        }

        // เอาไม้กั้น bossUI แบบเดิมออกไปได้เลย เพราะเราเช็คผ่านป้ายไฟ IntroDialog.isIntroActive แทนแล้วครับ

        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            if (settingsMenuPanel != null && settingsMenuPanel.activeSelf)
            {
                CloseSettings();
            }
            else if (isPaused)
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
        isPaused = true;
        isGamePaused = true; // 🌟 เปิดป้ายไฟเตือน!
        Time.timeScale = 0f;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        TogglePlayerInput(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        isGamePaused = false; // 🌟 ปิดป้ายไฟเตือน!
        Time.timeScale = 1f;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);

        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        TogglePlayerInput(true);
    }

    public void OpenSettings()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

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