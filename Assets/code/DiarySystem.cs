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

    [Header("ระบบเสียง (ลากไฟล์เสียง .mp3 หรือ .wav มาใส่)")]
    public AudioClip pickupSound;      // เสียงหยิบสมุด
    public AudioClip openDiarySound;   // เสียงเปิดหน้ากระดาษ
    public AudioClip closeDiarySound;  // เสียงปิดหน้ากระดาษ

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
        // 🌟 1. สั่งเล่นเสียงเก็บของตรงนี้! (เล่นเสียงตรงตำแหน่งที่สมุดวางอยู่)
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // 2. เปิดปุ่มสมุดในหน้าต่างกระเป๋า
        diaryInventoryButton.SetActive(true);

        // 3. ซ่อนโมเดลสมุดในฉาก
        diary3DModel.SetActive(false);

        canPickup = false;
    }

    // --- 2 ฟังก์ชันด้านล่างนี้ เอาไว้ไปผูกกับปุ่มคลิกใน UI ---

    // เปิดหน้าอ่าน (เอาไปตั้งค่าที่ OnClick ของ DiaryButton ในกระเป๋า)
    public void OpenDiary()
    {
        // 🌟 เล่นเสียงเปิดกระดาษ (ให้เสียงมาดังที่กล้องหลัก จะได้ยินชัดเจนแบบเสียง UI)
        if (openDiarySound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(openDiarySound, Camera.main.transform.position);
        }

        diaryReadPanel.SetActive(true);
    }

    // ปิดหน้าอ่าน (เอาไปตั้งค่าที่ OnClick ของ CloseButton)
    public void CloseDiary()
    {
        // 🌟 เล่นเสียงปิดกระดาษ
        if (closeDiarySound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(closeDiarySound, Camera.main.transform.position);
        }

        diaryReadPanel.SetActive(false);
    }
}