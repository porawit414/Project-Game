using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [Header("หน้าต่างกระเป๋าสนิม (UI)")]
    public GameObject inventoryPanel;

    // 🌟 1. เพิ่มช่องให้ลากหน้าต่าง Pause Menu มาใส่ เพื่อเช็คว่ามันเปิดอยู่ไหม 🌟
    [Header("หน้าต่าง Pause Menu (กันทับกัน)")]
    public GameObject pauseMenuPanel;

    [Header("เสียงเปิด/ปิดกระเป๋า")]
    public AudioClip openSound;
    public AudioClip closeSound;

    [Header("🌟 ลากสคริปต์หันกล้อง/เดิน มาใส่ช่องนี้เพื่อปิดตอนเปิดกระเป๋า 🌟")]
    public MonoBehaviour playerLookScript;
    public MonoBehaviour playerMoveScript;

    // 🌟 เปลี่ยนเป็น public เพื่อให้สคริปต์ Pause Menu แอบมามองเห็นได้
    public bool isInventoryOpen = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // 🌟 2. ถ้าหน้า Pause Menu เปิดอยู่ จะไม่ยอมให้กด Tab เปิดกระเป๋าเด็ดขาด 🌟
        if (pauseMenuPanel != null && pauseMenuPanel.activeSelf)
        {
            return; // หยุดการทำงานตรงนี้เลย ข้ามการกด Tab ไป
        }

        // เปิด/ปิด ด้วย Tab ตามปกติ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        // 🌟 3. ถ้ากระเป๋าเปิดอยู่ แล้วผู้เล่นกดปุ่ม ESC ให้ทำการ "ปิดกระเป๋า" 🌟
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        // สั่งปิด/เปิด สคริปต์กล้องและการเดิน
        if (playerLookScript != null) playerLookScript.enabled = !isInventoryOpen;
        if (playerMoveScript != null) playerMoveScript.enabled = !isInventoryOpen;

        // เล่นเสียง
        if (isInventoryOpen && openSound != null) audioSource.PlayOneShot(openSound);
        else if (!isInventoryOpen && closeSound != null) audioSource.PlayOneShot(closeSound);

        // จัดการลูกศรเมาส์
        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void CloseInventoryBtn()
    {
        if (isInventoryOpen) ToggleInventory();
    }
}