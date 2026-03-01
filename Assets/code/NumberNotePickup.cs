using UnityEngine;
using System.Collections;

public class NumberNotePickup : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject noteInventoryButton; // ปุ่มในกระเป๋า
    public GameObject noteReadPanel;       // หน้าต่างรูปกระดาษใบใหญ่
    public GameObject interactMessage;     // ป้ายกด F

    [Header("Ghost System")] // <--- ส่วนที่เพิ่มใหม่
    public GameObject ghostTrigger;        // ลากจุดดักผี (Ghost_Trigger) มาใส่ช่องนี้

    [Header("Audio")]
    public AudioClip pickupSound;

    private bool canPickup = false;
    private bool isReading = false;

    void Update()
    {
        // 1. กด F เพื่อเก็บ
        if (canPickup && Input.GetKeyDown(KeyCode.F) && !isReading)
        {
            PickUpNote();
        }

        // 2. ถ้าเปิดอ่านอยู่ แล้วกดปุ่มอื่น (เช่น Esc) ให้ปิด
        if (isReading && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            if (interactMessage != null) interactMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
            if (interactMessage != null) interactMessage.SetActive(false);
        }
    }

    void PickUpNote()
    {
        // เปิดปุ่มในกระเป๋า
        if (noteInventoryButton != null) noteInventoryButton.SetActive(true);

        // --- ส่วนสำคัญ: สั่งเปิดระบบผีหลอกทันทีที่เก็บกระดาษ ---
        if (ghostTrigger != null) 
        {
            ghostTrigger.SetActive(true); 
            Debug.Log("ระบบผีหลอกเปิดใช้งานแล้ว! เตรียมตัวหันหลังได้เลย...");
        }

        // เล่นเสียง
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // ซ่อนตัวกระดาษในฉาก 3D
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        if (interactMessage != null) interactMessage.SetActive(false);

        Debug.Log("เก็บกระดาษแล้ว!");
    }

    public void OpenNote()
    {
        if (noteReadPanel != null)
        {
            noteReadPanel.SetActive(true);
            isReading = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseNote()
    {
        if (noteReadPanel != null)
        {
            noteReadPanel.SetActive(false);
            isReading = false;
        }
    }
}