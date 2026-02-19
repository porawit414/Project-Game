using UnityEngine;
using System.Collections;

public class StorageDoorController : MonoBehaviour
{
    [Header("Settings")]
    public float openAngle = -90f;    // ปรับเฉพาะจุดนี้เป็น -90f ตามคำขอ
    public float smoothSpeed = 3f;    
    public float autoCloseDelay = 3f; 

    [Header("Collision")]
    public Collider blockingCollider; 

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
            if (!isOpen) 
            {
                OpenDoor();
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

        if (closeCoroutine != null) StopCoroutine(closeCoroutine);
        closeCoroutine = StartCoroutine(AutoCloseRoutine());
    }

    IEnumerator AutoCloseRoutine()
    {
        yield return new WaitForSeconds(autoCloseDelay);
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