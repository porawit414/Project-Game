using UnityEngine;

public class FinalChainDoor : MonoBehaviour
{
    [Header("การตั้งค่าประตู")]
    public Transform doorBody;        // ตัวบานประตู
    public float openAngle = 90f;     // องศาเปิด
    public float smoothSpeed = 3f;    // ความเร็ว

    [Header("ระบบล็อค (ลากโซ่มาใส่)")]
    public GameObject chainLock;      // ถ้าช่องนี้มีของ = ล็อค

    [Header("เสียง")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;     // เสียงตอนติดล็อค

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;
    private AudioSource audioSource;

    void Start()
    {
        if (doorBody == null) doorBody = transform;
        closedRot = doorBody.localRotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        Quaternion targetRot = isOpen ? openRot : closedRot;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }

    // ฟังก์ชันสั่งการ (เรียกจากตัวผู้เล่น)
    public void InteractWithDoor()
    {
        // 1. ถ้ายังมีโซ่ขวางอยู่
        if (chainLock != null) 
        {
            Debug.Log("ประตูล็อค! ติดโซ่");
            if (lockedSound != null) audioSource.PlayOneShot(lockedSound);
            return;
        }

        // 2. ถ้าไม่มีโซ่ -> สลับ เปิด/ปิด
        isOpen = !isOpen;

        if (isOpen)
        {
            if (openSound != null) audioSource.PlayOneShot(openSound);
        }
        else
        {
            if (closeSound != null) audioSource.PlayOneShot(closeSound);
        }
    }
}