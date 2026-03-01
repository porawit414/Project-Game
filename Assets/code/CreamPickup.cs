using UnityEngine;

public class CreamPickup : MonoBehaviour
{
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

            if (pickupMessage != null) pickupMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            playerCollider = null;

            if (pickupMessage != null) pickupMessage.SetActive(false);
        }
    }

    private void CollectCream()
    {
        // 🌟 1. ย้ายคำสั่งเปิดปุ่ม UI มาไว้บรรทัดแรกสุด! (ดักบัคเงียบ)
        if (creamInventoryButton != null)
        {
            creamInventoryButton.SetActive(true);
        }

        // 🌟 2. สั่งโชว์ข้อความแจ้งเตือน
        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowText("ได้รับ: " + itemName);
        }

        // 3. เล่นเสียง
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // 4. ซ่อนป้ายข้อความ
        if (pickupMessage != null) pickupMessage.SetActive(false);

        // 5. นำข้อมูลเข้ากระเป๋าหลัก (ถ้าระบบนี้พัง อย่างน้อยปุ่ม UI ด้านบนก็เปิดไปแล้ว)
        if (playerCollider != null)
        {
            SimpleInventory inventory = playerCollider.GetComponent<SimpleInventory>();
            ItemPickup itemData = GetComponent<ItemPickup>();
            if (inventory != null && itemData != null)
            {
                inventory.AddItem(itemData);
            }
        }

        // 6. ซ่อนโมเดลคีมตัดโซ่ในฉาก
        gameObject.SetActive(false);
    }
}