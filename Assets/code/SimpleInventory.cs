using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterAssets; // ต้องมีบรรทัดนี้

public class SimpleInventory : MonoBehaviour
{
    [Header("UI Control")]
    public GameObject inventoryPanel; 

    [Header("UI Slots")]
    public Transform itemContent;      
    public Transform documentContent;  
    public Transform evidenceContent;  
    
    [Header("Settings")]
    public GameObject inventoryItemPrefab; 

    [Header("Data")]
    public List<ItemPickup> collectedItems = new List<ItemPickup>(); 

    private bool isInventoryOpen = false;
    private StarterAssetsInputs _input; // ตัวแปรสำหรับคุยกับระบบเดิน

    void Start()
    {
        // ค้นหาตัวควบคุม StarterAssets ที่ตัวคน
        _input = GetComponent<StarterAssetsInputs>();

        if (inventoryPanel != null) 
        {
            inventoryPanel.SetActive(false);
            SetCursorState(false);
        }
    }

    void Update()
    {
        // กด I เพื่อเปิด/ปิด
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen;
            
            if (inventoryPanel != null) inventoryPanel.SetActive(isInventoryOpen);
            
            // สั่งจัดการเมาส์
            SetCursorState(isInventoryOpen);
        }
    }

    // ฟังก์ชันสั่งการเมาส์และกล้อง
    void SetCursorState(bool isOpen)
    {
        if (isOpen)
        {
            // เปิดกระเป๋า:
            // 1. บอก StarterAssets ว่า "ไม่ต้องหันหน้าตามเมาส์นะ"
            if (_input != null) 
            {
                _input.cursorInputForLook = false; 
                _input.cursorLocked = false;
            }

            // 2. ปลดล็อกเมาส์ให้เห็นชัดๆ
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // ปิดกระเป๋า:
            // 1. บอก StarterAssets ว่า "กลับมาหันหน้าตามปกติได้แล้ว"
            if (_input != null) 
            {
                _input.cursorInputForLook = true;
                _input.cursorLocked = true;
            }

            // 2. ซ่อนเมาส์
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // แก้บั๊ก Alt-Tab
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && isInventoryOpen)
        {
            SetCursorState(true);
        }
    }

    // --- ส่วนเก็บของ (เหมือนเดิม) ---
    public void AddItem(ItemPickup newItem)
    {
        collectedItems.Add(newItem);
        CreateItemButton(newItem);
    }

    void CreateItemButton(ItemPickup item)
    {
        Transform targetContent = null;
        switch (item.itemType)
        {
            case ItemType.GeneralItem: targetContent = itemContent; break;
            case ItemType.Document:    targetContent = documentContent; break;
            case ItemType.Evidence:    targetContent = evidenceContent; break;
        }

        if (targetContent != null)
        {
            GameObject newSlot = Instantiate(inventoryItemPrefab, targetContent);
            newSlot.transform.localScale = Vector3.one; 
            Image icon = newSlot.GetComponent<Image>();
            if (icon != null) icon.sprite = item.itemIcon;
        }
    }
}