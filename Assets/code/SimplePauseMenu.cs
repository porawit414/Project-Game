using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SimplePauseMenu : MonoBehaviour
{
    [Header("หน้าต่างเมนู (ลาก Pause Panel มาใส่)")]
    public GameObject pauseMenuPanel;

    // 🌟 1. เพิ่มช่องให้ลากสคริปต์กระเป๋ามาใส่ เพื่อให้มันคุยกันได้ 🌟
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
        isPaused = false;
        canPause = true;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // 🌟 2. ถ้าระบบกระเป๋าเปิดอยู่ จะข้ามคำสั่ง Pause ไปเลย ให้กระเป๋าทำงานแทน 🌟
        if (inventoryController != null && inventoryController.isInventoryOpen)
        {
            return; // หยุดการทำงานของ Update ไว้แค่นี้ ไม่ต้องเช็คการกด ESC ของ Pause
        }

        // ระบบ Pause ปกติ ทำงานก็ต่อเมื่อกระเป๋าปิดอยู่เท่านั้น
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
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
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        TogglePlayerInput(true);
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