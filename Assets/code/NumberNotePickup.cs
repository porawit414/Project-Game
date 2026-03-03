using UnityEngine;
using System.Collections;

public class NumberNotePickup : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของกระดาษโน้ต (ห้ามซ้ำ)")]
    public string noteSaveKey = "Item_NumberNote"; // ชื่อที่ใช้จำว่าเก็บกระดาษไปหรือยัง

    [Header("UI Settings")]
    public GameObject noteInventoryButton; // ปุ่มในกระเป๋า
    public GameObject noteReadPanel;       // หน้าต่างรูปกระดาษใบใหญ่
    public GameObject interactMessage;     // ป้ายกด F

    [Header("Ghost System")]
    public GameObject ghostTrigger;        // ลากจุดดักผี (Ghost_Trigger) มาใส่ช่องนี้

    [Header("Audio")]
    public AudioClip pickupSound;

    private bool canPickup = false;
    private bool isReading = false;

    void Start()
    {
        // 🌟 1. เช็คตอนเริ่มเกมว่า "เคยเก็บกระดาษโน้ตใบนี้ไปหรือยัง?"
        if (PlayerPrefs.GetInt(noteSaveKey, 0) == 1)
        {
            // เปิดปุ่มในกระเป๋าให้เลย
            if (noteInventoryButton != null) noteInventoryButton.SetActive(true);

            // ซ่อนตัวกระดาษในฉาก 3D 
            if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;
            if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;

            // หมายเหตุ: ไม่สั่งเปิด ghostTrigger ซ้ำ ให้ผีหลอกแค่ตอนเก็บครั้งแรกเท่านั้น 👻

            // ปิดสคริปต์นี้ไปเลย จะได้ไม่ทำงานซ้ำซ้อน
            this.enabled = false;
        }
    }

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
        if (GetComponent<MeshRenderer>() != null) gameObject.GetComponent<MeshRenderer>().enabled = false;
        if (GetComponent<Collider>() != null) gameObject.GetComponent<Collider>().enabled = false;

        if (interactMessage != null) interactMessage.SetActive(false);

        // 🌟 2. เซฟลงเครื่องว่า "เก็บกระดาษโน้ตใบนี้ไปแล้ว! (ค่า = 1)"
        PlayerPrefs.SetInt(noteSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บกระดาษแล้ว!");

        // ปิดสคริปต์การเก็บของ
        this.enabled = false;
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