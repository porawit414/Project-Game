using UnityEngine;

// เก็บ Enum ไว้เหมือนเดิม (ถ้าลบไป เดี๋ยว Type จะ Error)
public enum ItemType { GeneralItem, Document, Evidence }

public class ItemPickup : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;

    // ❌ ลบ void OnTriggerEnter ทิ้งไปแล้ว!
    // ตอนนี้สคริปต์นี้จะมีหน้าที่แค่ "ถือข้อมูล" รอให้ KeyPickup มาดึงไปใช้ครับ
}