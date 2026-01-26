using UnityEngine;

// สร้างตัวเลือก 3 แบบ
public enum ItemType { GeneralItem, Document, Evidence }

public class ItemPickup : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SimpleInventory inventory = other.GetComponent<SimpleInventory>();
            if (inventory != null)
            {
                inventory.AddItem(this);
                Destroy(gameObject);
            }
        }
    }
}