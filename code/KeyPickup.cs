using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // เช็คว่าคนที่เดินมาชน มี Tag ชื่อ "Player" หรือไม่
        if (other.CompareTag("Player"))
        {
            // เข้าถึงสคริปต์ Inventory ของผู้เล่น
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            
            if (inventory != null)
            {
                inventory.hasKey = true; // เปลี่ยนสถานะเป็นมีกุญแจ
                Debug.Log("เก็บกุญแจแล้ว!"); // เช็คใน Console
                Destroy(gameObject); // ลบกุญแจออกจากฉาก
            }
        }
    }
}