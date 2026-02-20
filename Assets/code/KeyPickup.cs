using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip pickupSound;

    [Header("ลาก KeyMessageCanvas มาใส่ช่องนี้")]
    public GameObject pickupMessage;

    [Header("ลากปุ่มไอคอนในกระเป๋า (UI) มาใส่ช่องนี้")]
    public GameObject inventoryButtonUI; // <--- 🌟 (จุดที่ 1) เพิ่มตัวแปรรองรับปุ่ม UI ของเรา

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && pickupMessage != null)
        {
            pickupMessage.SetActive(true); // เดินเข้า -> โชว์ป้าย
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && pickupMessage != null)
        {
            pickupMessage.SetActive(false); // เดินออก -> ซ่อนป้าย
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SimpleInventory inventory = other.GetComponent<SimpleInventory>();
                ItemPickup itemData = GetComponent<ItemPickup>();

                if (inventory != null && itemData != null)
                {
                    inventory.AddItem(itemData);
                    if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                    // ซ่อนป้ายก่อน
                    if (pickupMessage != null) pickupMessage.SetActive(false);

                    // 🌟 (จุดที่ 2) สั่งเปิดปุ่มไอคอนในกระเป๋าสนิมของเรา!
                    if (inventoryButtonUI != null) inventoryButtonUI.SetActive(true);

                    Destroy(gameObject);
                }
            }
        }
    }
}