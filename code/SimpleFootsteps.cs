using UnityEngine;

public class SimpleFootsteps : MonoBehaviour
{
    public AudioSource audioSource;    // ตัวลำโพง
    public AudioClip walkSound;        // ไฟล์เสียงเดิน
    public float stepRate = 0.5f;      // ความถี่เสียง (เดินเร็ว/ช้า)

    private CharacterController character;
    private float nextStepTime;

    void Start()
    {
        // หาตัวควบคุมการเดินอัตโนมัติ
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ถ้าตัวละครยืนบนพื้น และ มีความเร็ว (กำลังขยับ)
        if (character.isGrounded && character.velocity.magnitude > 0.1f)
        {
            // ถ้าถึงเวลาเล่นเสียงรอบถัดไป
            if (Time.time >= nextStepTime)
            {
                // เล่นเสียง (และสุ่มระดับเสียงนิดหน่อยให้ดูสมจริง)
                audioSource.pitch = Random.Range(0.8f, 1.1f);
                audioSource.PlayOneShot(walkSound);
                
                // ตั้งเวลาเล่นเสียงรอบหน้า
                nextStepTime = Time.time + stepRate;
            }
        }
    }
}