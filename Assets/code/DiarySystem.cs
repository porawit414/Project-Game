using UnityEngine;
using UnityEngine.UI;

public class DiarySystem : MonoBehaviour
{
    [Header("🌟 ตั้งชื่อเซฟของไดอารี่ (ห้ามซ้ำกับของชิ้นอื่น!)")]
    public string diarySaveKey = "Diary_01"; // ตัวแปรนี้คือชื่อความจำของสมุดเล่มนี้ครับ

    [Header("โมเดลสมุดในฉาก")]
    public GameObject diary3DModel;

    [Header("ปุ่มไดอารี่ในกระเป๋า (UI)")]
    public GameObject diaryInventoryButton;

    [Header("หน้าต่างโชว์ภาพไดอารี่ (UI)")]
    public GameObject diaryReadPanel;

    [Header("ระบบเสียง (ลากไฟล์เสียง .mp3 หรือ .wav มาใส่)")]
    public AudioClip pickupSound;
    public AudioClip openDiarySound;
    public AudioClip closeDiarySound;

    private bool canPickup = false;

    void Start()
    {
        // 🌟 1. เช็คตอนเริ่มเกมว่า "เคยเก็บไดอารี่เล่มนี้ไปหรือยัง?"
        // ถ้า PlayerPrefs จำได้ว่า diarySaveKey มีค่าเป็น 1 แปลว่าเคยเก็บแล้ว
        if (PlayerPrefs.GetInt(diarySaveKey, 0) == 1)
        {
            // จัดการเปิด-ปิดของให้เหมือนตอนเก็บไปแล้ว
            if (diary3DModel != null) diary3DModel.SetActive(false); // ซ่อนสมุดบนโต๊ะ
            if (diaryInventoryButton != null) diaryInventoryButton.SetActive(true); // โชว์ปุ่มในกระเป๋า

            // ปิดการทำงานของสคริปต์นี้ไปเลย จะได้ไม่ต้องเดินชนให้เสียเวลา
            this.enabled = false;
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            PickUpDiary();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }

    void PickUpDiary()
    {
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        if (NotificationManager.instance != null)
        {
            NotificationManager.instance.ShowText("ได้รับ: สมุดไดอารี่");
        }

        diaryInventoryButton.SetActive(true);
        diary3DModel.SetActive(false);
        canPickup = false;

        // 🌟 2. เซฟลงเครื่องว่า "เก็บไดอารี่เล่มนี้แล้วนะ! (ให้ค่าเป็น 1)"
        PlayerPrefs.SetInt(diarySaveKey, 1);
        PlayerPrefs.Save();

        // ปิดสคริปต์กันเหนียว ไม่ให้กด F ซ้ำได้อีก
        this.enabled = false;
    }

    public void OpenDiary()
    {
        if (openDiarySound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(openDiarySound, Camera.main.transform.position);
        }

        diaryReadPanel.SetActive(true);
    }

    public void CloseDiary()
    {
        if (closeDiarySound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(closeDiarySound, Camera.main.transform.position);
        }

        diaryReadPanel.SetActive(false);
    }
}