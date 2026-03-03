using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของกุญแจดอกนี้ (ห้ามซ้ำกันถ้ามีหลายดอก)")]
    public string keySaveKey = "Item_Key_1";

    public AudioClip pickupSound;

    [Header("ลาก KeyMessageCanvas มาใส่ช่องนี้")]
    public GameObject pickupMessage;

    [Header("ลากปุ่มไอคอนในกระเป๋า (UI) มาใส่ช่องนี้")]
    public GameObject inventoryButtonUI;

    private void Start()
    {
        // 🌟 1. เช็คตอนเริ่มเกมว่า "เคยเก็บกุญแจดอกนี้ไปหรือยัง?"
        // ถ้า PlayerPrefs จำได้ว่ามีค่าเป็น 1 แปลว่าเคยเก็บแล้ว
        if (PlayerPrefs.GetInt(keySaveKey, 0) == 1)
        {
            // เปิดปุ่มกุญแจในกระเป๋าให้เลย
            if (inventoryButtonUI != null) inventoryButtonUI.SetActive(true);

            // สั่งทำลายกุญแจในฉากทิ้งไปเลย จะได้ไม่ต้องเดินมาเก็บซ้ำ
            Destroy(gameObject);
        }
    }

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

                    // สั่งเปิดปุ่มไอคอนในกระเป๋าสนิมของเรา!
                    if (inventoryButtonUI != null) inventoryButtonUI.SetActive(true);

                    // 🌟 2. เซฟความจำลงเครื่องว่าเก็บกุญแจนี้ไปแล้ว! (ค่า = 1)
                    PlayerPrefs.SetInt(keySaveKey, 1);
                    PlayerPrefs.Save();

                    Destroy(gameObject); // ทำลายทิ้งตอนเก็บสำเร็จ
                }
            }
        }
    }
}