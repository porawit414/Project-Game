using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [Header("ความเร็วในการเลื่อน")]
    public float scrollSpeed = 50f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // 🌟 --- เพิ่มส่วนนี้: ค้นหาระบบ PauseMenu และสั่งล็อคทันทีที่ฉากจบเริ่ม! --- 🌟
        SimplePauseMenu pauseMenu = FindObjectOfType<SimplePauseMenu>();
        if (pauseMenu != null)
        {
            pauseMenu.canPause = false; // 1. ล็อคกุญแจ ห้ามกด ESC เด็ดขาด!
            pauseMenu.TogglePlayerInput(false); // 2. แช่แข็งผู้เล่น ห้ามเดิน/ห้ามหันหน้า

            // 3. (แถมให้) ปลดล็อคเมาส์โชว์ขึ้นมา เผื่อเพื่อนคุณมีปุ่ม "กลับเมนูหลัก" ให้กดตอนเครดิตเลื่อนจบ
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        // สั่งให้ขยับขึ้นแกน Y (ด้านบน) เรื่อยๆ
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}