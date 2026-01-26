using UnityEngine;
using StarterAssets; // *บรรทัดสำคัญ! ต้องมีอันนี้เพื่อสั่งงานตัวละคร

public class InventoryToggle : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject inventoryPanel; 

    [Header("Player Setup")]
    // ลาก PlayerCapsule มาใส่ช่องนี้ (เพื่อสั่งหยุดหมุนคอ)
    public StarterAssetsInputs playerInput; 

    void Start()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        
        // เริ่มเกมมาล็อคเมาส์
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        bool isOpening = !inventoryPanel.activeSelf;
        
        // 1. เปิด/ปิด หน้าต่างกระเป๋า
        inventoryPanel.SetActive(isOpening);

        // 2. หยุดเวลา (ศัตรูหยุดเดิน)
        Time.timeScale = isOpening ? 0f : 1f;

        // 3. ปลดล็อคเมาส์ (ให้เห็นลูกศร)
        Cursor.visible = isOpening;
        Cursor.lockState = isOpening ? CursorLockMode.None : CursorLockMode.Locked;

        // 4. สั่งตัวละคร "หยุดรับค่าการมอง" (แก้หน้าหมุนตามเมาส์)
        if (playerInput != null)
        {
            playerInput.cursorInputForLook = !isOpening; 
            // แปลว่า: ถ้าเปิดกระเป๋า = ห้ามหมุนกล้อง, ถ้าปิดกระเป๋า = หมุนได้ปกติ
        }
    }
}