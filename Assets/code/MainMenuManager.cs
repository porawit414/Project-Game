using UnityEngine;
using UnityEngine.SceneManagement; // ต้องมีบรรทัดนี้เพื่อใช้คำสั่งเปลี่ยนฉาก

public class MainMenuManager : MonoBehaviour
{
    // ฟังก์ชันนี้เอาไว้ผูกกับปุ่ม "New Game" (เริ่มเล่นใหม่)
    public void StartNewGame()
    {
        // 1. สั่งล้างสมอง ลบตำแหน่งเซฟเก่าทิ้งให้หมด!
        PlayerPrefs.DeleteKey("SavedPlayerX");
        PlayerPrefs.DeleteKey("SavedPlayerY");
        PlayerPrefs.DeleteKey("SavedPlayerZ");

        Debug.Log("🗑️ ลบเซฟเก่าทิ้งแล้ว กำลังเริ่มเกมใหม่...");

        // 2. โหลดเข้าฉากเกม DemoScene
        SceneManager.LoadScene("DemoScene");
    }

    // แถมให้: ฟังก์ชันนี้เอาไว้ผูกกับปุ่ม "Continue" (เล่นต่อ)
    public void ContinueGame()
    {
        // โหลดเข้าฉากเกม DemoScene เลย ไม่ต้องลบเซฟ พอโหลดเสร็จสคริปต์ PlayerLoadPosition จะดึงตัวละครไปที่เดิมเอง
        SceneManager.LoadScene("DemoScene");
    }
}