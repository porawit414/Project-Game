using UnityEngine;
using System.Collections;

public class DelayedMusicController : MonoBehaviour
{
    [Header("การตั้งค่าเสียงเพลง")]
    public AudioSource audioSource;       // ลำโพงที่จะใช้เล่นเสียง
    public AudioClip backgroundMusic;     // ไฟล์เพลงที่ต้องการเล่น
    
    [Header("ตั้งค่าเวลา")]
    [Tooltip("ใส่ตัวเลข 120 เพื่อให้รอ 2 นาที (หน่วยเป็นวินาที)")]
    public float delayTime = 120f;        // เวลาที่หน่วงก่อนเล่น (120 วิ = 2 นาที)

    void Start()
    {
        // ถ้าคุณจอนไม่ได้ลาก AudioSource มาใส่ สคริปต์จะสร้างลำโพงให้เอง
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ปิดการเล่นอัตโนมัติตอนเริ่มฉาก (เพื่อไม่ให้มันดังขึ้นมาทันที)
        audioSource.playOnAwake = false;
        
        // นำไฟล์เพลงไปใส่ในลำโพง
        audioSource.clip = backgroundMusic;
        
        // ตั้งให้เล่นวนซ้ำไปเรื่อยๆ (Loop)
        audioSource.loop = true; 
        
        // เริ่มนับเวลาถอยหลัง
        StartCoroutine(PlayMusicAfterDelay());
    }

    IEnumerator PlayMusicAfterDelay()
    {
        // สั่งให้ระบบหยุดรอเป็นเวลา 120 วินาที
        yield return new WaitForSeconds(delayTime);
        
        // พอครบเวลาปุ๊บ สั่งให้ลำโพงเล่นเพลงทันที
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("แจ้งเตือน: ลืมลากไฟล์เพลงใส่ช่อง Background Music ครับคุณจอน!");
        }
    }
}