using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject inventoryPanel; 

    [Header("Mouse Look Setup")]
    // ลาก PlayerCameraRoot มาใส่ช่องนี้เพื่อให้หน้าจอนิ่ง
    public GameObject cameraObject; 

    void Start()
    {
        // เริ่มเกมมาให้ปิดกระเป๋าก่อน
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        
        // เซ็ตให้เมาส์หายไปตอนเริ่มเกม
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // กด Tab เพื่อเปิด/ปิดกระเป๋า
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        // เช็คสถานะปัจจุบันของกระเป๋า
        bool isOpening = !inventoryPanel.activeSelf;
        
        // 1. เปิด/ปิด แผงกระเป๋า
        inventoryPanel.SetActive(isOpening);

        // 2. หยุดเวลาในเกม (เพื่อให้ตัวละครเดินไม่ได้)
        Time.timeScale = isOpening ? 0f : 1f;

        // 3. จัดการเรื่องเมาส์ (โชว์เมาส์เมื่อเปิดกระเป๋า)
        Cursor.visible = isOpening;
        Cursor.lockState = isOpening ? CursorLockMode.None : CursorLockMode.Locked;

        // 4. สั่ง "ปิด" ตัวคุมกล้อง (เพื่อให้หน้าจอนิ่ง 100%)
        if (cameraObject != null)
        {
            cameraObject.SetActive(!isOpening);
        }
    }
}