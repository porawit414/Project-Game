using UnityEngine;
using System.Collections;

public class StorageDoorController : MonoBehaviour
{
    [Header("Settings")]
    public float openAngle =  90f;    // มุมเปิด
    public float smoothSpeed = 3f;    // ความเร็ว
    public float autoCloseDelay = 3f; // เวลาหน่วงก่อนปิดเอง (วินาที)

    [Header("Collision")]
    public Collider blockingCollider; // ลาก Box Collider ที่กั้นคนมาใส่ตรงนี้

    private bool isOpen = false;
    private bool isPlayerNearby = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;
    private Coroutine closeCoroutine;

    void Start()
    {
        closedRotation = transform.localRotation;
        targetRotation = closedRotation;
    }

    void Update()
    {
        // --- 1. ส่วนรับคำสั่งเปิด ---
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen) // ถ้าประตูยังไม่เปิด ให้เปิด
            {
                OpenDoor();
            }
            // (ตัดระบบกดปิดเองออก เพราะจะให้ปิดอัตโนมัติอย่างเดียว)
        }

        // --- 2. ส่วนขยับประตู ---
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);

        // --- 3. ส่วนเช็คการเดินทะลุ (สำคัญ!) ---
        // เช็คว่าประตู "ปิดสนิท" หรือยัง? (มุมห่างจากตอนปิดไม่เกิน 1 องศา)
        float angleDiff = Quaternion.Angle(transform.localRotation, closedRotation);

        if (!isOpen && angleDiff < 1f)
        {
            // ถ้าสั่งปิดแล้ว และประตูหมุนมาจนเกือบสนิท -> เปิด Collider ให้แข็ง (กั้นคน)
            if (blockingCollider != null) blockingCollider.enabled = true;
        }
        else
        {
            // ถ้าเปิดอยู่ หรือกำลังหมุน -> ปิด Collider (ให้เดินทะลุได้)
            if (blockingCollider != null) blockingCollider.enabled = false;
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        targetRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);

        // เริ่มนับถอยหลังปิดเอง
        if (closeCoroutine != null) StopCoroutine(closeCoroutine);
        closeCoroutine = StartCoroutine(AutoCloseRoutine());
    }

    IEnumerator AutoCloseRoutine()
    {
        // รอ 3 วินาที
        yield return new WaitForSeconds(autoCloseDelay);

        // สั่งปิด
        isOpen = false;
        targetRotation = closedRotation;
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