using UnityEngine;
using System.Collections; // ต้องมีบรรทัดนี้เพื่อใช้ระบบหน่วงเวลา (Coroutine)

public class GhostAudioDelay : MonoBehaviour
{
    [Header("ตั้งค่าเสียงผีเดินผ่าน")]
    public AudioClip passBySound; // ช่องสำหรับเอาไฟล์เสียงมาใส่ใน Inspector
    public float soundDelay = 0f; // ตั้งเวลาหน่วง 0.2 วินาที ตามที่คุณต้องการ
    
    private AudioSource audioSource; // ตัวแปรสำหรับคุมเครื่องเล่นเสียง

    // ฟังก์ชัน Start จะทำงาน "ทันที" ที่ตัวผีถูกสั่งให้โผล่มา (SetActive เป็น true)
    void Start()
    {
        // 1. สร้างเครื่องเล่นเสียงแปะติดไว้กับตัวผี
        audioSource = gameObject.AddComponent<AudioSource>(); 
        
        // 2. เอาไฟล์เสียงที่เตรียมไว้ใส่เข้าไปในเครื่องเล่น
        audioSource.clip = passBySound; 
        
        // 3. ปิดไม่ให้มันเล่นเองทันที เพราะเราจะใช้ระบบหน่วงเวลา
        audioSource.playOnAwake = false; 
        
        // 4. ปรับเสียงให้เป็นระบบ 3 มิติ (สำคัญมาก: ผู้เล่นจะได้ยินเสียงวิ่งผ่านหน้าจากซ้ายไปขวาจริงๆ)
        audioSource.spatialBlend = 1f; 

        // 5. ถ้ามีไฟล์เสียงใส่ไว้ ให้เริ่มนับถอยหลังหน่วงเวลาได้เลย
        if (passBySound != null)
        {
            StartCoroutine(PlayDelayedSound());
        }
    }

    // ฟังก์ชันสำหรับหน่วงเวลา
    IEnumerator PlayDelayedSound()
    {
        // สั่งให้โค้ดหยุดรอเป็นเวลา 0.2 วินาที (ตามค่า soundDelay)
        yield return new WaitForSeconds(soundDelay);
        
        // พอครบ 0.2 วินาที เช็คว่าถ้าเครื่องเล่นเสียงยังอยู่ (ผียังไม่โดนลบ)
        if (audioSource != null)
        {
            audioSource.Play(); // สั่งเล่นเสียงหลอน!
        }
    }
    
    // หมายเหตุ: เมื่อผีถูกคำสั่ง Destroy(targetGhost, 1.7f) จากกล่องกับดักทำลายทิ้ง 
    // ตัว AudioSource ในสคริปต์นี้จะถูกทำลายตามไปด้วย ทำให้เสียงตัดดับไปทันทีครับ
}