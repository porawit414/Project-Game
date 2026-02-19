using UnityEngine;

public class GhostActivator : MonoBehaviour
{
    // ลากตัวผีที่ซ่อนอยู่ (Ghostly Woman 2) มาใส่ในช่องนี้
    public GameObject targetGhost; 
    
    // ปรับเวลาให้ผีโชว์ตัวสั้นลงเหลือ 1.7 วินาที (เพื่อให้รับกับความเร็วที่เพิ่มขึ้น)
    public float displayDuration = 1.7f;

    private void OnTriggerEnter(Collider other)
    {
        // เช็กว่าสิ่งที่เดินชนคือผู้เล่นที่มี Tag ว่า Player หรือไม่
        if (other.CompareTag("Player"))
        {
            // 1. สั่งให้ผีปรากฏตัวออกมาคลาน
            targetGhost.SetActive(true);

            // 2. สั่งให้ทำลายผีทิ้งหลังจากผ่านไป 1.7 วินาที
            Destroy(targetGhost, displayDuration);

            // 3. ทำลายกล่องกับดักทิ้งทันที เพื่อไม่ให้เกิดเหตุการณ์ซ้ำ
            Destroy(gameObject);
        }
    }
}