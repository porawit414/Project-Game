using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    [Header("ใส่ของตรงนี้")]
    public GameObject loadingScreen;
    public VideoPlayer videoPlayer;

    // 🌟 เปลี่ยนชื่อด่านในนี้ให้ตรงกับชื่อไฟล์ด่านเกมของคุณ (เช่น "DemoScene")
    private string gameSceneName = "DemoScene";

    public void StartNewGame()
    {
        // 💣 ล้างบางเซฟทั้งหมด! ไม่ว่าจะเป็นตำแหน่งผู้เล่น, ไอเทม, หลักฐาน หรือตัวเลข
        // คำสั่งนี้คำสั่งเดียว ล้างเกลี้ยงทั้งเกมครับ!
        PlayerPrefs.DeleteAll();

        PlayerPrefs.Save(); // ย้ำให้ระบบเซฟการลบทิ้ง

        Debug.Log("🗑️ นิวเคลียร์ลง! ล้างความจำทุกอย่างเรียบร้อย กำลังเริ่มเกมใหม่...");
        Time.timeScale = 1f;

        // โหลดเข้าฉากเกม
        StartCoroutine(LoadLevel(gameSceneName));
    }

    public void PlayGame()
    {
        Debug.Log("🔄 กำลังโหลดเซฟเดิม...");
        Time.timeScale = 1f;

        // โหลดเข้าฉากเกม
        StartCoroutine(LoadLevel(gameSceneName));
    }

    public void QuitGame()
    {
        Debug.Log("ออกเกมแล้ว");
        Application.Quit();
    }

    IEnumerator LoadLevel(string sceneName)
    {
        loadingScreen.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }

        yield return new WaitForSeconds(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}