using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    [Header("ใส่ของตรงนี้")]
    public GameObject loadingScreen;
    public VideoPlayer videoPlayer;

    // 🌟 เพิ่มช่องสำหรับลากปุ่ม "เริ่มใหม่" มาใส่
    [Header("ปุ่มเมนู")]
    public GameObject newGameButton;

    // เปลี่ยนชื่อด่านในนี้ให้ตรงกับชื่อไฟล์ด่านเกมของคุณ (เช่น "DemoScene")
    private string gameSceneName = "DemoScene";

    void Start()
    {
        // เช็คว่ามีเซฟเกมหรือไม่ (สมมติให้คีย์ชื่อ "HasSave" ถ้ามีจะเป็น 1 ถ้าไม่มีจะเป็น 0)
        if (PlayerPrefs.GetInt("HasSave", 0) == 1)
        {
            // ถ้ามีเซฟ: แสดงปุ่มเริ่มใหม่
            newGameButton.SetActive(true);
        }
        else
        {
            // ถ้าไม่มีเซฟ (เข้าเกมครั้งแรก): ซ่อนปุ่มเริ่มใหม่
            newGameButton.SetActive(false);
        }
    }

    public void PlayGame()
    {
        // เมื่อกดปุ่มเล่นเกม (ปุ่มหลัก)
        if (PlayerPrefs.GetInt("HasSave", 0) == 1)
        {
            Debug.Log("🔄 มีเซฟอยู่แล้ว กำลังโหลดเซฟเดิมมาเล่นต่อ...");
        }
        else
        {
            Debug.Log("🆕 เข้าเกมครั้งแรก! เริ่มต้นเกมใหม่...");
            // เพื่อความชัวร์ จะสั่งลบค่าที่อาจตกค้างอยู่ด้วยก็ได้
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        Time.timeScale = 1f;
        StartCoroutine(LoadLevel(gameSceneName));
    }

    public void StartNewGame()
    {
        // 💣 ล้างบางเซฟทั้งหมด! ไม่ว่าจะเป็นตำแหน่งผู้เล่น, ไอเทม, หลักฐาน หรือตัวเลข
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); // ย้ำให้ระบบเซฟการลบทิ้ง

        Debug.Log("🗑️ นิวเคลียร์ลง! ล้างความจำทุกอย่างเรียบร้อย กำลังเริ่มเกมใหม่...");
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