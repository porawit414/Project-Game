using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // <--- เพิ่มบรรทัดนี้เพื่อใช้คำสั่งเปลี่ยนฉาก

public class SimplePauseMenu : MonoBehaviour
{
    [Header("หน้าต่างเมนู (ลาก Pause Panel มาใส่)")]
    public GameObject pauseMenuPanel;

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

    // 🌟 ฟังก์ชันใหม่: กดแล้ววาร์ปกลับไปหน้าเมนูหลัก (บ้านสยองขวัญ)
    public void GoToMainMenu()
    {
        Debug.Log("🏠 กำลังกลับไปหน้าเมนูหลัก...");
        Time.timeScale = 1f; // สำคัญมาก! ต้องคืนค่าเวลาเป็นปกติก่อนเปลี่ยนฉาก

        // ใส่ชื่อฉากเมนูของคุณ (จากรูปก่อนๆ คือ "MainMen")
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
        Application.Quit(); // ใช้ตอน build เกมเสร็จแล้ว

        // บรรทัดนี้ช่วยให้กดทดสอบใน Unity Editor แล้วมันหยุดรันเกมให้ด้วย
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}