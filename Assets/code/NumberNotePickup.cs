using UnityEngine;

public class NumberNotePickup : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject noteInventoryButton; // ปุ่มในกระเป๋า
    public GameObject noteReadPanel;       // หน้าต่างรูปกระดาษใบใหญ่
    public GameObject interactMessage;     // ป้ายกด F

    [Header("Audio")]
    public AudioClip pickupSound;

    private bool canPickup = false;
    private bool isReading = false;

    void Update()
    {
        // 1. กด F เพื่อเก็บ
        if (canPickup && Input.GetKeyDown(KeyCode.F) && !isReading)
        {
            PickUpNote();
        }

        // 2. ถ้าเปิดอ่านอยู่ แล้วกดปุ่มอื่น (เช่น Esc หรือ E) ให้ปิดก็ได้ (Option เสริม)
        if (isReading && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            if (interactMessage != null) interactMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
            if (interactMessage != null) interactMessage.SetActive(false);
        }
    }

    void PickUpNote()
    {
        // เปิดปุ่มในกระเป๋า
        if (noteInventoryButton != null) noteInventoryButton.SetActive(true);

        // เล่นเสียง
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // ซ่อนตัวกระดาษในฉาก 3D
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        if (interactMessage != null) interactMessage.SetActive(false);

        Debug.Log("เก็บกระดาษแล้ว!");
    }

    // --- ฟังก์ชันใหม่ที่คุณต้องการ ---

    public void OpenNote()
    {
        if (noteReadPanel != null)
        {
            noteReadPanel.SetActive(true);
            isReading = true;

            // ปลดล็อคเมาส์ให้กดปิดได้
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseNote()
    {
        if (noteReadPanel != null)
        {
            noteReadPanel.SetActive(false);
            isReading = false;

            // ลบโค้ด Lock Mouse ทิ้ง หรือคอมเมนต์ไว้แบบนี้:
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;

            // หมายเหตุ: ลูกศรจะยังโชว์อยู่จนกว่าคุณจะกดปิดหน้าต่างกระเป๋า (Inventory)
        }
    }
}