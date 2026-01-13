using UnityEngine;

public class FlashlightSystem : MonoBehaviour
{
    // ตัวไฟฉาย (ต้องลากจาก Hierarchy เท่านั้น)
    public GameObject flashlightObject; 
    
    // ตัวจ่ายเสียง
    public AudioSource audioSource;     

    [Header("ใส่เสียงตรงนี้")]
    public AudioClip soundTurnOn;       // เสียงตอนเปิด
    public AudioClip soundTurnOff;      // เสียงตอนปิด

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            // 1. เช็คสถานะปัจจุบัน (เปิดอยู่ไหม?)
            bool isActive = flashlightObject.activeSelf;

            // 2. สลับสถานะ (ถ้าเปิด->ปิด, ถ้าปิด->เปิด)
            flashlightObject.SetActive(!isActive);

            // 3. เล่นเสียงให้ถูกอัน
            if (audioSource != null)
            {
                if (!isActive) // กำลังจะเปิด (เพราะเดิมมันปิดอยู่)
                {
                    audioSource.PlayOneShot(soundTurnOn);
                }
                else // กำลังจะปิด
                {
                    audioSource.PlayOneShot(soundTurnOff);
                }
            }
        }
    }
}