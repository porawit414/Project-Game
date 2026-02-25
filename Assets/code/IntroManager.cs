using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement; // 🌟 ต้องมีบรรทัดนี้เพื่อใช้คำสั่งเปลี่ยนฉาก

public class IntroManager : MonoBehaviour
{
    [Header("ใส่ชิ้นส่วน UI")]
    public GameObject blackScreen;
    public GameObject videoScreen;
    public VideoPlayer videoPlayer;

    [Header("ตั้งค่าการโหลดฉาก")]
    public string gameplaySceneName = "DemoScene"; // 🌟 พิมพ์ชื่อฉากเกมเพลย์ของคุณให้ตรงเป๊ะๆ
    public float delayAfterVideo = 2f; // ค้างจอดำกี่วินาทีก่อนเข้าเกม

    void Start()
    {
        // เริ่มมาให้จอดำทึบก่อน และซ่อนวิดีโอไว้
        blackScreen.SetActive(true);
        videoScreen.SetActive(false);

        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // 🌟 จอดำก่อนเริ่ม 1 วินาที (ให้เกมตั้งตัว)
        yield return new WaitForSeconds(1f);

        // 🌟 โชว์จอวิดีโอและสั่งเล่น
        videoScreen.SetActive(true);
        videoPlayer.Play();

        yield return new WaitForSeconds(0.5f); // รอระบบวิดีโอสตาร์ท

        // 🌟 รอจนกว่าวิดีโอจะเล่นจบ
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // 🌟 วิดีโอเล่นจบแล้ว! ปิดจอวิดีโอทิ้ง จะเหลือแค่จอดำ
        videoScreen.SetActive(false);

        // 🌟 ค้างจอดำทิ้งไว้ (เช่น 2 วินาที) ให้คนเล่นได้ลุ้น
        yield return new WaitForSeconds(delayAfterVideo);

        // 🌟 วาร์ป! โหลดไปหน้าเกมเพลย์เลย!
        SceneManager.LoadScene(gameplaySceneName);
    }
}