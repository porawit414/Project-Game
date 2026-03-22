using UnityEngine;

public class Trigger_Ghost_Waving : MonoBehaviour
{
    [Header("ลากตัวผีที่มีสคริปต์ GhostAudioDelay มาใส่")]
    public GhostAudioDelay ghostScript; // เปลี่ยนจาก GhostController เป็น GhostAudioDelay ให้ตรงกับของจริง

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 1. เช็คว่าเป็นผู้เล่น และยังไม่เคยทำงาน
        if (other.CompareTag("Player") && !hasTriggered)
        {
            // 2. เช็คว่าผีโผล่มาหรือยัง และมีสคริปต์ติดอยู่ไหม
            if (ghostScript != null && ghostScript.gameObject.activeSelf)
            {
                // สั่งให้ผีเริ่มวิ่ง (ฟังก์ชันนี้เราเขียนเพิ่มไว้ใน GhostAudioDelay แล้ว)
                ghostScript.StartRunning(); 
                
                Debug.Log("🎯 ผู้เล่นเดินชนจุดดัก: สั่งผีวิ่งใส่แล้ว!");
                
                hasTriggered = true; 
                // ทำลายจุดดักทิ้งเพื่อไม่ให้ทำงานซ้ำ
                Destroy(gameObject, 0.1f); 
            }
        }
    }
}