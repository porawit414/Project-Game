using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    [Header("ใส่ของตรงนี้")]
    public GameObject loadingScreen;
    public VideoPlayer videoPlayer;

    [Header("ปุ่มที่ต้องการซ่อน/โชว์")]
    public GameObject newGameButton;

    private string gameSceneName = "DemoScene";

    void Start()
    {
        // 1. เช็คว่ามีเซฟไหม? ถ้ามีโชว์ปุ่มเริ่มใหม่ ถ้าไม่มีให้ซ่อนไว้
        if (PlayerPrefs.HasKey("HasSave"))
        {
            if (newGameButton != null) newGameButton.SetActive(true);
        }
        else
        {
            if (newGameButton != null) newGameButton.SetActive(false);
        }
    }

    // 🔴 ผูกกับปุ่ม "เริ่มใหม่"
    public void StartNewGame()
    {
        // ล้างข้อมูลเซฟทั้งหมดในเครื่องทิ้งแบบถอนรากถอนโคน!
        PlayerPrefs.DeleteAll();

        // ส่งจดหมายไปบอกด่านเกมว่า "รอบนี้เริ่มใหม่นะ ไม่ต้องโหลดเซฟ"
        PlayerPrefs.SetInt("IsLoadGame", 0);
        PlayerPrefs.Save();

        StartCoroutine(LoadLevel(gameSceneName));
    }

    // 🟢 ผูกกับปุ่ม "เล่นเกม"
    public void PlayGame()
    {
        // ส่งจดหมายไปบอกด่านเกมว่า "รอบนี้ให้ดึงเซฟมาเล่นต่อได้เลย"
        PlayerPrefs.SetInt("IsLoadGame", 1);
        PlayerPrefs.Save();

        StartCoroutine(LoadLevel(gameSceneName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(string sceneName)
    {
        loadingScreen.SetActive(true);
        if (videoPlayer != null) videoPlayer.Play();
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone) yield return null;
    }
}