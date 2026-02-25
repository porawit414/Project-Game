using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class GameCutscene : MonoBehaviour
{
    [Header("ลากของมาใส่ตรงนี้")]
    public GameObject videoScreen;  // จอ Raw Image
    public VideoPlayer videoPlayer; // เครื่องเล่นวิดีโอ
    public GameObject chatSystem;   // ระบบแชทที่โชว์ชื่อ "อังเดร ไนท์ฟอร์ด"

    void Start()
    {
        // 1. เริ่มฉากมา ให้ซ่อนแชทไว้ก่อน
        if (chatSystem != null) chatSystem.SetActive(false);

        // 2. สั่งเล่นวิดีโอทันที
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        videoScreen.SetActive(true);
        videoPlayer.Play();

        yield return new WaitForSeconds(0.5f); // รอวิดีโอสตาร์ทนิดนึง

        // 3. รอจนกว่าวิดีโอจะเล่นจบ
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // 4. วิดีโอจบ ปิดจอวิดีโอทิ้งไปเลย
        videoScreen.SetActive(false);

        // 5. เปิดระบบแชทให้ตัวหนังสือ "อังเดร ไนท์ฟอร์ด" เด้งขึ้นมา!
        if (chatSystem != null) chatSystem.SetActive(true);
    }
}