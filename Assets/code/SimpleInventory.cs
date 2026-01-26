using UnityEngine;          // เรียกใช้ชุดคำสั่งพื้นฐานของ Unity
using UnityEngine.UI;       // เรียกใช้ชุดคำสั่งเกี่ยวกับ UI (เช่น Image, Text)
using System.Collections.Generic; // เรียกใช้ระบบ List (การเก็บข้อมูลเป็นรายการ)

public class SimpleInventory : MonoBehaviour
{
    // [Header] คือการทำป้ายหัวข้อโชว์ในหน้า Inspector ให้ดูง่ายๆ ครับ
    [Header("UI Slots (โซนกำหนดที่วางของ)")]
    // ตัวแปรสำหรับระบุว่า "กล่อง" ไหนคือที่เก็บ "ไอเทมทั่วไป" (ลาก Column_Item มาใส่ช่องนี้)
    public Transform itemContent;      
    
    // ตัวแปรสำหรับระบุว่า "กล่อง" ไหนคือที่เก็บ "เอกสาร" (ลาก Column_Document มาใส่ช่องนี้)
    public Transform documentContent;  
    
    // ตัวแปรสำหรับระบุว่า "กล่อง" ไหนคือที่เก็บ "หลักฐาน" (ลาก Column_Evidence มาใส่ช่องนี้)
    public Transform evidenceContent;  
    
    [Header("Settings (ตั้งค่าแม่แบบ)")]
    // ตัวแปรนี้สำคัญมาก! คือ "แม่พิมพ์" (Prefab) ของรูปไอคอนที่จะให้เกมสร้างขึ้นมา
    // ต้องลาก Prefab ที่เป็น Image มาใส่ช่องนี้ ไม่งั้นเกมไม่รู้ว่าจะสร้างรูปหน้าตาแบบไหน
    public GameObject inventoryItemPrefab; 

    [Header("Data (ข้อมูลในกระเป๋า)")]
    // List เอาไว้เก็บรายชื่อของทั้งหมดที่เราเก็บมาแล้ว (เก็บเป็นข้อมูลเฉยๆ ไม่เกี่ยวกับภาพ)
    // เอาไว้ใช้เวลาจะเช็คว่า "มีกุญแจดอกนี้หรือยัง" หรือเอาไปทำระบบ Save เกม
    public List<ItemPickup> collectedItems = new List<ItemPickup>(); 

    // ฟังก์ชันนี้จะถูกเรียกใช้งานตอนที่ "ตัวละครเดินชนของ" (จากสคริปต์ ItemPickup)
    // newItem คือ ข้อมูลของชิ้นที่เราเพิ่งเก็บได้
    public void AddItem(ItemPickup newItem)
    {
        // 1. เพิ่มข้อมูลของชิ้นนั้นลงสมุดบันทึกรายการ (List)
        collectedItems.Add(newItem);
        
        // 2. สั่งให้สร้าง "ปุ่มรูปภาพ" โชว์บนหน้าจอกระเป๋า
        CreateItemButton(newItem);
    }

    // ฟังก์ชันสำหรับสร้างรูปไอคอนบนหน้าจอ UI
    void CreateItemButton(ItemPickup item)
    {
        // สร้างตัวแปรมารอรับค่าว่า "สรุปแล้วชิ้นนี้ต้องไปอยู่กล่องไหน"
        Transform targetContent = null;
        
        // ใช้คำสั่ง switch เพื่อแยกประเภทของ (เหมือนเครื่องคัดแยกไปรษณีย์)
        switch (item.itemType)
        {
            // ถ้าเป็นของทั่วไป -> ให้เป้าหมายคือตะกร้า itemContent
            case ItemType.GeneralItem: targetContent = itemContent; break;
            
            // ถ้าเป็นเอกสาร -> ให้เป้าหมายคือตะกร้า documentContent
            case ItemType.Document:    targetContent = documentContent; break;
            
            // ถ้าเป็นหลักฐาน -> ให้เป้าหมายคือตะกร้า evidenceContent
            case ItemType.Evidence:    targetContent = evidenceContent; break;
        }

        // ตรวจสอบว่า "หาตะกร้าลงได้ไหม?" (ถ้า targetContent ไม่ว่าง แสดงว่าหาเจอ)
        if (targetContent != null)
        {
            // คำสั่ง Instantiate คือการ "โคลนนิ่ง" หรือเสกของขึ้นมาใหม่
            // โดยเสกจากแม่พิมพ์ (inventoryItemPrefab) เอาไปแปะไว้ในตะกร้าเป้าหมาย (targetContent)
            // ตัวแปร newSlot คือตัวแทนของไอคอนชิ้นใหม่ที่เพิ่งเสกออกมาสดๆ ร้อนๆ
            GameObject newSlot = Instantiate(inventoryItemPrefab, targetContent);
            
            // ดึงชิ้นส่วน Image (รูปภาพ) ออกมาจากไอคอนที่เพิ่งสร้าง เพื่อจะเตรียมเปลี่ยนรูป
            Image icon = newSlot.GetComponent<Image>();
            
            // เช็คกันพลาด: ดูว่าใน Prefab นั้นมี Component ชื่อ Image จริงไหม?
            if (icon != null)
            {
                // ถ้ามีจริง -> ก็ทำการเอารูปจากของที่เราเก็บ (item.itemIcon) 
                // มาสวมทับแทนที่รูปเดิมของ Prefab (icon.sprite)
                // ผลลัพธ์คือ จากรูปสี่เหลี่ยมเปล่าๆ ก็จะกลายเป็นรูปกุญแจ หรือรูปเอกสารตามจริง
                icon.sprite = item.itemIcon;
            }
        }
    }
}