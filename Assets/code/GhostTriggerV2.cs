using UnityEngine;

public class GhostTriggerV2 : MonoBehaviour
{
    // ลากตัวผีที่มีสคริปต์ GhostRushSystem มาใส่ช่องนี้
    public GhostRushSystem ghostScript; 

    private void OnTriggerEnter(Collider other)
    {
        // เช็คว่าคนที่เดินชนมี Tag ว่า Player หรือไม่
        if (other.CompareTag("Player"))
        {
            if (ghostScript != null)
            {
                ghostScript.StartRunning(); // สั่งให้ผีเริ่มวิ่ง
                Debug.Log("🎯 ผู้เล่นชนจุดดัก: สั่งผีวิ่งแล้ว!");
                
                // ชนแล้วให้ทำลายจุดดักทิ้งเลย จะได้ไม่ทำงานซ้ำ
                Destroy(gameObject); 
            }
        }
    }
}