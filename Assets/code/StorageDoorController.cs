using UnityEngine;
using System.Collections;

public class StorageDoorController : MonoBehaviour
{
    [Header("Settings")]
    public float openAngle = -90f;
    public float smoothSpeed = 3f;
    public float autoCloseDelay = 3f;

    [Header("Collision")]
    public Collider blockingCollider;

    // 🌟 --- เพิ่มระบบเสียงตรงนี้ --- 🌟
    [Header("Audio Settings")]
    public AudioSource doorAudio;     // ช่องใส่ Audio Source
    public AudioClip openSound;       // เสียงตอนเปิด
    public AudioClip closeSound;      // เสียงตอนปิด

    private bool isOpen = false;
    private bool isPlayerNearby = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;
    private Coroutine closeCoroutine;

    void Start()
    {
        closedRotation = transform.localRotation;
        targetRotation = closedRotation;

        // ดึง Audio Source มาใส่อัตโนมัติ (ถ้าลืมลากใส่)
        if (doorAudio == null)
        {
            doorAudio = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // --- 1. ส่วนรับคำสั่งเปิด/ปิด (กด E สลับไปมา) ---
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen)
            {
                CloseDoor(); // ถ้าเปิดอยู่ -> ให้ปิด
            }
            else
            {
                OpenDoor();  // ถ้าปิดอยู่ -> ให้เปิด
            }
        }

        // --- 2. ส่วนขยับประตู ---
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);

        // --- 3. ส่วนเช็คการเดินทะลุ ---
        float angleDiff = Quaternion.Angle(transform.localRotation, closedRotation);

        if (!isOpen && angleDiff < 1f)
        {
            if (blockingCollider != null) blockingCollider.enabled = true;
        }
        else
        {
            if (blockingCollider != null) blockingCollider.enabled = false;
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        targetRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);

        // 🌟 เล่นเสียงเปิดประตู 🌟
        if (doorAudio != null && openSound != null)
        {
            doorAudio.PlayOneShot(openSound);
        }

        // รีเซ็ตการนับเวลาปิดอัตโนมัติใหม่
        if (closeCoroutine != null) StopCoroutine(closeCoroutine);
        closeCoroutine = StartCoroutine(AutoCloseRoutine());
    }

    void CloseDoor()
    {
        isOpen = false;
        targetRotation = closedRotation;

        // 🌟 เล่นเสียงปิดประตู 🌟
        if (doorAudio != null && closeSound != null)
        {
            doorAudio.PlayOneShot(closeSound);
        }

        // 🛑 ยกเลิกการนับเวลาปิดอัตโนมัติ (เพราะผู้เล่นชิงปิดไปก่อนแล้ว)
        if (closeCoroutine != null) StopCoroutine(closeCoroutine);
    }

    IEnumerator AutoCloseRoutine()
    {
        // รอเวลาตามที่ตั้งไว้
        yield return new WaitForSeconds(autoCloseDelay);
        
        // ถ้าถึงเวลาแล้วประตูยังเปิดอยู่ ให้สั่งปิด
        if (isOpen)
        {
            CloseDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }
}