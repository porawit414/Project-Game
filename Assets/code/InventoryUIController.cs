using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [Header("หน้าต่างกระเป๋าสนิม (UI)")]
    public GameObject inventoryPanel;

    [Header("เสียงเปิด/ปิดกระเป๋า")]
    public AudioClip openSound;
    public AudioClip closeSound;

    [Header("🌟 ลากสคริปต์หันกล้อง/เดิน มาใส่ช่องนี้เพื่อปิดตอนเปิดกระเป๋า 🌟")]
    public MonoBehaviour playerLookScript; // สคริปต์ที่ใช้หันหน้า/หมุนกล้อง
    public MonoBehaviour playerMoveScript; // สคริปต์ที่ใช้เดิน (ถ้าอยากให้หยุดเดินด้วยตอนเปิดกระเป๋า)

    private bool isInventoryOpen = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventory();
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);

        // 🌟 สั่งปิด/เปิด สคริปต์กล้องและการเดิน 🌟
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