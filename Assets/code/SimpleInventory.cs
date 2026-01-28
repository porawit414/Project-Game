using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterAssets;

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
    private StarterAssetsInputs _input;

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();

        if (inventoryPanel != null) 
        {
            inventoryPanel.SetActive(false);
            SetCursorState(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryOpen = !isInventoryOpen;
            if (inventoryPanel != null) inventoryPanel.SetActive(isInventoryOpen);
            SetCursorState(isInventoryOpen);
        }
    }

    void SetCursorState(bool isOpen)
    {
        if (isOpen)
        {
            if (_input != null) 
            {
                _input.cursorInputForLook = false; 
                _input.cursorLocked = false;
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            if (_input != null) 
            {
                _input.cursorInputForLook = true;
                _input.cursorLocked = true;
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && isInventoryOpen)
        {
            SetCursorState(true);
        }
    }

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

    // --- ส่วนที่แก้ไข (เปลี่ยน I ใหญ่ เป็น i เล็ก) ---
    public bool HasItem(string nameToCheck)
    {
        foreach (ItemPickup item in collectedItems)
        {
            // แก้ตรงนี้ครับ! ใช้ itemName (ตัวเล็ก)
            if (item.itemName == nameToCheck || item.name == nameToCheck) 
            {
                return true;
            }
        }
        return false;
    }
}