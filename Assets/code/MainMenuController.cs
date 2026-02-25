using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // <--- บรรทัดนี้สำคัญมาก! เพื่อคุมวิดีโอ

public class MainMenuController : MonoBehaviour
{
    [Header("ใส่ของตรงนี้")]
    public GameObject loadingScreen; // เอาไว้เก็บหน้าจอโหลด (Raw Image)
    public VideoPlayer videoPlayer;  // เอาไว้เก็บตัวเล่นวิดีโอ

    public void PlayGame()
    {
        // 1. สั่งให้เวลาเดินปกติ (เผื่อมันหยุดอยู่) << เพิ่มบรรทัดนี้ครับ
        Time.timeScale = 1f;

        // 2. เริ่มโหลด
        StartCoroutine(LoadLevel(1));
    }
    public void QuitGame()
    {
        Debug.Log("ออกเกมแล้ว");
        Application.Quit();
    }

    // ฟังก์ชันโหลดฉาก
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