using UnityEngine;
using UnityEngine.UI;

public class DiarySystem : MonoBehaviour
{
    [Header("โมเดลสมุดในฉาก")]
    public GameObject diary3DModel;

    [Header("ปุ่มไดอารี่ในกระเป๋า (UI)")]
    public GameObject diaryInventoryButton;

    [Header("หน้าต่างโชว์ภาพไดอารี่ (UI)")]
    public GameObject diaryReadPanel;

    private bool canPickup = false;

    void Update()
    {
        // ถ้าผู้เล่นอยู่ใกล้ (canPickup เป็นจริง) และกดปุ่ม F
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            PickUpDiary();
        }
    }

    // เช็คว่าผู้เล่นเดินมาชนกล่อง Trigger ของสมุดหรือยัง
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            // ตรงนี้สามารถเพิ่มโค้ดให้โชว์ข้อความ "กด F เพื่อเก็บ" ได้
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
            // ซ่อนข้อความ "กด F เพื่อเก็บ"
        }
    }

    void PickUpDiary()
    {
        // --- จุดที่แก้ไขแล้ว ---
        // 1. เปิดปุ่มสมุดในหน้าต่างกระเป๋าก่อน! (สคริปต์จะได้ทำงานบรรทัดนี้จนจบ)
        diaryInventoryButton.SetActive(true);

        // 2. ค่อยซ่อนโมเดลสมุดในฉากเป็นลำดับสุดท้าย (เพราะถ้าปิดตัวเองก่อน โค้ดจะหยุดทำงานทันที)
        diary3DModel.SetActive(false);

        canPickup = false;
    }

    // --- 2 ฟังก์ชันด้านล่างนี้ เอาไว้ไปผูกกับปุ่มคลิกใน UI ---

    // เปิดหน้าอ่าน (เอาไปตั้งค่าที่ OnClick ของ DiaryButton ในกระเป๋า)
    public void OpenDiary()
    {
        diaryReadPanel.SetActive(true);
        // Time.timeScale = 0f; // เอาคอมเมนต์ออกถ้าอยากให้เกมหยุดชั่วคราวตอนอ่าน
    }

    // ปิดหน้าอ่าน (เอาไปตั้งค่าที่ OnClick ของ CloseButton)
    public void CloseDiary()
    {
        diaryReadPanel.SetActive(false);
        // Time.timeScale = 1f; // เอาคอมเมนต์ออกถ้าอยากให้เกมเดินต่อ
    }
}