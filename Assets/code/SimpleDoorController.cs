using UnityEngine;
using System.Collections;

public class SimpleDoorController : MonoBehaviour
{
    [Header("Settings")]
    public Transform doorBody;
    public float openAngle = 90f; 
    public float smoothSpeed = 3f;
    public GameObject doorUI;

    [Header("Auto Close & Physics")]
    public float autoCloseDelay = 3f; // ตั้งค่าไว้ 3 วินาที
    public Collider blockingCollider; // ลาก Box Collider ตัวที่ใช้กันทาง (ไม่ติ๊ก Is Trigger) มาใส่

    [Header("Audio Settings")]
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private bool isPlayerNearby = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private AudioSource audioSource;
    private Coroutine currentCoroutine;

    void Start()
    {
        if (doorBody == null) doorBody = transform;
        closedRotation = doorBody.localRotation;
        
        // --- แก้ไขตรงนี้ครับ ---
        // เอาเครื่องหมายลบ (-) ออก เพื่อให้เปิดไปฝั่งตรงข้าม
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0); 
        // -------------------

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        
        if (doorUI != null) doorUI.SetActive(false);

        // เริ่มเกม: ถ้าประตูปิดอยู่ ต้องแข็ง (ทะลุไม่ได้)
        if (blockingCollider != null) blockingCollider.enabled = true;
    }

    void Update()
    {
        // 1. ตรวจสอบการกด E เพื่อเปิด
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen) OpenDoor();
        }

        // 2. คำนวณการหมุนราบรื่น
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRotation, Time.deltaTime * smoothSpeed);

        // 3. Logic การเดินทะลุ
        float angleRemaining = Quaternion.Angle(doorBody.localRotation, closedRotation);
        
        if (angleRemaining > 0.1f) 
        {
            // ประตูกำลังเปิด หรือ กำลังปิด -> ปิด Collider (ทะลุได้)
            if (blockingCollider != null && blockingCollider.enabled)
                blockingCollider.enabled = false;
        }
        else if (!isOpen && angleRemaining <= 0.1f)
        {
            // ประตูปิดสนิทแล้ว และสถานะคือ Close -> เปิด Collider (ทะลุไม่ได้)
            if (blockingCollider != null && !blockingCollider.enabled)
                blockingCollider.enabled = true;
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        PlaySound(openSound);

        // ปิดการชนทันทีที่เริ่มเปิด
        if (blockingCollider != null) blockingCollider.enabled = false;

        // เริ่มนับถอยหลัง 3 วินาทีเพื่อปิดเอง
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(AutoCloseRoutine());
    }

    void CloseDoor()
    {
        isOpen = false;
        PlaySound(closeSound);
        // Collider จะกลับมาทำงานเองใน Update() เมื่อหมุนถึงจุดปิดสนิท
    }

    IEnumerator AutoCloseRoutine()
    {
        // รอ 3 วินาทีตามที่กำหนด
        yield return new WaitForSeconds(autoCloseDelay);
        CloseDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (doorUI != null) doorUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (doorUI != null) doorUI.SetActive(false);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}