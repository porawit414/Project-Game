using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SimpleInventory : MonoBehaviour
{
    [Header("UI Slots")]
    public Transform itemContent;      // ตะกร้าไอเทม
    public Transform documentContent;  // ตะกร้าเอกสาร
    public Transform evidenceContent;  // ตะกร้าหลักฐาน
    
    [Header("Data")]
    public List<ItemPickup> collectedItems = new List<ItemPickup>(); 

    public void AddItem(ItemPickup newItem)
    {
        collectedItems.Add(newItem);
        CreateItemButton(newItem);
    }

    void CreateItemButton(ItemPickup item)
    {
        Transform targetContent = null;
        // เช็คว่าของประเภทไหน ให้ไปลงตะกร้านั้น
        switch (item.itemType)
        {
            case ItemType.GeneralItem: targetContent = itemContent; break;
            case ItemType.Document:    targetContent = documentContent; break;
            case ItemType.Evidence:    targetContent = evidenceContent; break;
        }

        if (targetContent != null)
        {
            // สร้างช่องเก็บของใหม่ในตะกร้า
            GameObject newSlot = new GameObject("ItemSlot", typeof(Image));
            newSlot.transform.SetParent(targetContent, false); 
            
            // เอารูปไอคอนมาใส่
            Image icon = newSlot.GetComponent<Image>();
            icon.sprite = item.itemIcon;
            
            // ขยายปุ่มให้เต็มช่อง (ถ้าต้องการ)
            // หรือปล่อยไว้ให้ Grid Layout จัดการขนาดเองก็ได้
        }
    }
}