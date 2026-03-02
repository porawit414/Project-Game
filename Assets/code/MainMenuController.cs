using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // <--- บรรทัดนี้สำคัญมาก! เพื่อคุมวิดีโอ

public class MainMenuController : MonoBehaviour
{
    [Header("ใส่ของตรงนี้")]
    public GameObject loadingScreen; // เอาไว้เก็บหน้าจอโหลด (Raw Image)
    public VideoPlayer videoPlayer;  // เอาไว้เก็บตัวเล่นวิดีโอ

    // 🌟 1. ฟังก์ชันนี้เอาไว้ผูกกับปุ่ม "เริ่มเล่นใหม่ (New Game)"
    public void StartNewGame()
    {
        // สั่งล้างสมอง ลบตำแหน่งเซฟเก่าทิ้งให้หมด!
        PlayerPrefs.DeleteKey("SavedPlayerX");
        PlayerPrefs.DeleteKey("SavedPlayerY");
        PlayerPrefs.DeleteKey("SavedPlayerZ");
        PlayerPrefs.Save(); // ย้ำให้เซฟการลบทิ้ง

        Debug.Log("🗑️ ลบเซฟเก่าทิ้งแล้ว กำลังเริ่มเกมใหม่...");

        Time.timeScale = 1f;

        // เริ่มโหลดฉาก (พร้อมเปิดวิดีโอ)
        StartCoroutine(LoadLevel(1));
    }

    // 🌟 2. ฟังก์ชันนี้คือปุ่ม "เล่นเกม / เล่นต่อ (Continue)" ของเดิมของคุณ
    public void PlayGame()
    {
        Debug.Log("🔄 กำลังโหลดเซฟเดิม...");

        // สั่งให้เวลาเดินปกติ (เผื่อมันหยุดอยู่)
        Time.timeScale = 1f;

        // เริ่มโหลดฉากเลย ไม่ต้องลบเซฟ
        StartCoroutine(LoadLevel(1));
    }

    public void QuitGame()
    {
        Debug.Log("ออกเกมแล้ว");
        Application.Quit();
    }

    // ฟังก์ชันโหลดฉาก (ระบบวิดีโอโหลดดิ้งตัวเดิมของคุณ)
    IEnumerator LoadLevel(int sceneIndex)
    {
        // 1. เปิดหน้าจอโหลดขึ้นมาบังจอ
        loadingScreen.SetActive(true);

        // 2. สั่งให้วิดีโอเริ่มเล่น
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }

        // 3. เริ่มโหลดฉากเกมแบบเบื้องหลัง
        // รอ 1 วินาทีก่อนโหลดจริง (เพื่อให้คนดูวิดีโอทันโหลดหน่อย)
        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // รอจนกว่าจะโหลดเสร็จ
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}