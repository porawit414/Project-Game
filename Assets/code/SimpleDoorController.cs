using UnityEngine;
using System.Collections;

public class SimpleDoorController : MonoBehaviour
{
    [Header("Settings")]
    public Transform doorBody; 
    public float openAngle = 90f;
    public float smoothSpeed = 3f;

    [Header("Audio Settings")]
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private AudioSource audioSource;
    private BoxCollider doorCollider;

    void Start()
    {
        if (doorBody == null) doorBody = transform;
        closedRotation = doorBody.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
        audioSource = GetComponent<AudioSource>();
        doorCollider = GetComponent<BoxCollider>(); 
        
        if (doorCollider != null) doorCollider.isTrigger = false; 
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRotation, Time.deltaTime * smoothSpeed);

        // --- ส่วนที่แก้ไข: เช็กว่าถ้าประตูปิดเกือบสนิทแล้ว ถึงจะทำให้มันแข็ง ---
        if (doorCollider != null)
        {
            float angleDifference = Quaternion.Angle(doorBody.localRotation, closedRotation);
            
            if (isOpen) 
            {
                // ถ้าประตูเปิดอยู่ ให้ทะลุได้เสมอ
                doorCollider.isTrigger = true;
            }
            else if (angleDifference < 1f) 
            {
                // ถ้าประตูกำลังปิด และเหลืออีกไม่ถึง 1 องศาจะปิดสนิท ให้กลับมาแข็ง
                doorCollider.isTrigger = false;
            }
            else 
            {
                // ถ้าประตูกำลังเคลื่อนที่ปิด (ยังไม่สนิท) ให้ยังคงทะลุได้อยู่
                doorCollider.isTrigger = true;
            }
        }
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (audioSource != null)
        {
            AudioClip clipToPlay = isOpen ? openSound : closeSound;
            if (clipToPlay != null) audioSource.PlayOneShot(clipToPlay);
        }

        if (isOpen)
        {
            StopAllCoroutines();
            StartCoroutine(AutoCloseTimer(4f));
        }
    }

    IEnumerator AutoCloseTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isOpen)
        {
            ToggleDoor();
        }
    }
}