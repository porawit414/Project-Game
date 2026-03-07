using UnityEngine;

public class CreamPickup : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของไอเทมชิ้นนี้ (ห้ามซ้ำ)")]
    public string creamSaveKey = "Item_BoltCutter"; // ชื่อที่จะใช้จำว่าเก็บคีมไปหรือยัง

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

    private void Start()
    {
        // 🌟 1. เช็คตอนเริ่มเกมว่า "เคยเก็บคีมตัดโซ่ไปหรือยัง?"
        if (PlayerPrefs.GetInt(creamSaveKey, 0) == 1)
        {
            // เปิดปุ่มในกระเป๋า UI ให้เลย
            if (creamInventoryButton != null) creamInventoryButton.SetActive(true);

            // 🌟 จุดที่เพิ่มเข้ามา: แอบยัดข้อมูลคีมเข้ากระเป๋าผู้เล่นตอนโหลดเซฟด้วย!
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                SimpleInventory inventory = player.GetComponent<SimpleInventory>();
                ItemPickup itemData = GetComponent<ItemPickup>();
                if (inventory != null && itemData != null)
                {
                    inventory.AddItem(itemData); // ใส่กลับเข้ากระเป๋า ระบบจะได้จำได้ว่ามีของ!
                }
            }

            // ซ่อนโมเดลคีมในฉากทิ้งไปเลย จะได้ไม่ต้องเดินมาเก็บซ้ำ
            gameObject.SetActive(false);
        }
    }

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
        // 1. เปิดปุ่ม UI ในกระเป๋า
        if (creamInventoryButton != null)
        {
            creamInventoryButton.SetActive(true);
        }

        // 2. สั่งโชว์ข้อความแจ้งเตือน
        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowText("ได้รับ: " + itemName);
        }

        // 3. เล่นเสียง
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // 4. ซ่อนป้ายข้อความ
        if (pickupMessage != null) pickupMessage.SetActive(false);

        // 5. นำข้อมูลเข้ากระเป๋าหลัก 
        if (playerCollider != null)
        {
            SimpleInventory inventory = playerCollider.GetComponent<SimpleInventory>();
            ItemPickup itemData = GetComponent<ItemPickup>();
            if (inventory != null && itemData != null)
            {
                inventory.AddItem(itemData);
            }
        }

        // 🌟 6. เซฟลงเครื่องว่า "เก็บคีมตัดโซ่ไปแล้วนะ! (ค่า = 1)"
        PlayerPrefs.SetInt(creamSaveKey, 1);
        PlayerPrefs.Save();

        // 7. ซ่อนโมเดลคีมตัดโซ่ในฉาก
        gameObject.SetActive(false);
    }
}