using UnityEngine;

public class CreamPickup : MonoBehaviour
{
    // 🌟 เพิ่มช่องตั้งชื่อไอเทมสำหรับโชว์แจ้งเตือน
    [Header("ชื่อไอเทมที่จะโชว์ตอนแจ้งเตือน")]
    public string itemName = "คีมตัดโซ่";

    [Header("เสียงตอนเก็บครีม (ถ้ามี)")]
    public AudioClip pickupSound;

    [Header("ป้ายข้อความ 'กด F เพื่อเก็บ'")]
    public GameObject pickupMessage;

    [Header("🌟 ลากปุ่ม CreamButton จากกระเป๋ามาใส่ตรงนี้ 🌟")]
    public GameObject creamInventoryButton;

    private bool isPlayerNear = false;
    private Collider playerCollider;

    private void Update()
    {
        // ถ้าผู้เล่นอยู่ใกล้ๆ และกดปุ่ม F
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            CollectCream();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerCollider = other;

            // เดินเข้าใกล้ -> โชว์ป้าย
            if (pickupMessage != null) pickupMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            playerCollider = null;

            // เดินออกห่าง -> ซ่อนป้าย
            if (pickupMessage != null) pickupMessage.SetActive(false);
        }
    }

    private void CollectCream()
    {
        // 🌟 0. สั่งโชว์ข้อความแจ้งเตือนตรงนี้!
        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowText("ได้รับ: " + itemName);
        }

        // 1. นำข้อมูลเข้ากระเป๋าหลัก (เพื่อไม่ให้ระบบเดิม Error)
        if (playerCollider != null)
        {
            SimpleInventory inventory = playerCollider.GetComponent<SimpleInventory>();
            ItemPickup itemData = GetComponent<ItemPickup>();
            if (inventory != null && itemData != null)
            {
                inventory.AddItem(itemData);
            }
        }

        // 2. เล่นเสียง
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // 3. ซ่อนป้ายข้อความ
        if (pickupMessage != null) pickupMessage.SetActive(false);

        // 4. 🌟 สั่งเปิดปุ่มไอคอนครีม
        creamInventoryButton.SetActive(true);

        // 5. 🌟 เปลี่ยนจากลบทิ้ง เป็นแค่ "ซ่อน"
        gameObject.SetActive(false);
    }
}