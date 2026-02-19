using UnityEngine;
using TMPro; // ถ้าใช้ TextMeshPro
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // ทำ Singleton ให้เรียกใช้ง่ายๆ

    [Header("UI References")]
    public GameObject inventoryPanel;
    public GameObject readingPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;

    // เก็บรายการไดอารี่ที่มี
    private List<DiaryData> collectedDiaries = new List<DiaryData>();

    private void Awake() { Instance = this; }

    // ฟังก์ชันรับไดอารี่เข้ากระเป๋า
    public void AddDiary(DiaryData newDiary)
    {
        collectedDiaries.Add(newDiary);
        Debug.Log("เก็บไดอารี่: " + newDiary.diaryName + " ลงกระเป๋าแล้ว!");
        // TODO: ตรงนี้คุณสามารถสั่งให้สร้างปุ่ม UI ในหน้า InventoryPanel ได้
    }

    // ฟังก์ชันเมื่อกดปุ่มในกระเป๋าเพื่ออ่าน
    public void ReadDiary(DiaryData diaryToRead)
    {
        inventoryPanel.SetActive(false); // ปิดหน้ากระเป๋าชั่วคราว
        readingPanel.SetActive(true);    // เปิดหน้าอ่านกระเป๋า

        titleText.text = diaryToRead.diaryName;
        contentText.text = diaryToRead.diaryContent;
    }

    // ฟังก์ชันปิดหน้าอ่าน
    public void CloseReadingPanel()
    {
        readingPanel.SetActive(false);
    }
}