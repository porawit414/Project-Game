using UnityEngine;
using System.Collections; // ต้องมีบรรทัดนี้เพื่อใช้งานระบบนับเวลา (Coroutine)

public class FinalChainDoor : MonoBehaviour
{
    // 🌟 --- เพิ่มตัวแปรเซฟความจำตรงนี้ --- 🌟
    [Header("🌟 ชื่อเซฟของประตูโซ่ (ตั้งให้ไม่ซ้ำกัน!)")]
    public string doorSaveID = "Chain_Door_1";

    [Header("การตั้งค่าประตู")]
    public Transform doorBody;        // ตัวบานประตู
    public float openAngle = 90f;     // องศาเปิด
    public float smoothSpeed = 3f;    // ความเร็ว
    public float autoCloseTime = 3f;  // เวลาที่จะให้ประตูปิดเอง (วินาที)

    [Header("ระบบล็อค (ลากโซ่มาใส่)")]
    public GameObject chainLock;      // ถ้าช่องนี้มีของ = ล็อค

    [Header("เสียง")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;     // เสียงตอนติดล็อค

    [Header("ระบบฟิสิกส์ (ป้องกันเดินติด)")]
    public Collider solidDoorCollider; // ลากบานประตูมาใส่ช่องนี้

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;
    private AudioSource audioSource;
    private Coroutine autoCloseCoroutine; // ตัวช่วยจำสถานะการนับเวลา

    // ตัวแปรเช็คว่าระบบบันทึกการตัดโซ่ไปแล้วหรือยัง
    private bool isChainCutSaved = false;

    void Start()
    {
        if (doorBody == null) doorBody = transform;
        closedRot = doorBody.localRotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // 🌟 1. เช็คความจำตอนโหลดฉาก: ประตูบานนี้เคยถูกตัดโซ่ไปแล้วหรือยัง?
        if (PlayerPrefs.GetInt(doorSaveID, 0) == 1)
        {
            isChainCutSaved = true; // ล็อคไว้ไม่ให้เซฟซ้ำ

            // ถ้าเคยตัดแล้ว ก็ทำลายโมเดลโซ่ทิ้งไปเลยตั้งแต่เริ่มเกม!
            if (chainLock != null)
            {
                Destroy(chainLock);
            }
            Debug.Log("ประตู " + doorSaveID + " เคยถูกตัดโซ่แล้ว วันนี้เปิดได้เลย!");
        }
    }

    void Update()
    {
        // 🌟 2. ระบบแอบดูอัตโนมัติ: ถ้าโซ่ถูกทำลาย (เป็น null) แต่ยังไม่ได้เซฟ -> ให้เซฟทันที!
        if (chainLock == null && !isChainCutSaved)
        {
            isChainCutSaved = true;
            PlayerPrefs.SetInt(doorSaveID, 1);
            PlayerPrefs.Save();
            Debug.Log("โซ่ถูกตัดแล้ว! ระบบจำไว้แล้วว่าไม่ต้องเอาโซ่มาขวางอีก");
        }

        // 1. จัดการเรื่องการหมุนของประตู
        Quaternion targetRot = isOpen ? openRot : closedRot;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRot, Time.deltaTime * smoothSpeed);

        // 2. ระบบป้องกันเดินติด (แก้ไขใหม่ให้กด E ซ้ำได้)
        if (solidDoorCollider != null)
        {
            if (isOpen)
            {
                // เปลี่ยนเป็นวิญญาณ (เดินทะลุได้ แต่เป้าเล็งยังตรวจจับเพื่อกด E ได้)
                solidDoorCollider.isTrigger = true;
            }
            else
            {
                // เช็คว่าประตูสวิงกลับมาปิดสนิทหรือยัง
                if (Quaternion.Angle(doorBody.localRotation, closedRot) < 2f)
                {
                    solidDoorCollider.isTrigger = false;  // ปิดสนิทแล้ว กลับมาแข็งเหมือนเดิม
                }
                else
                {
                    solidDoorCollider.isTrigger = true;   // ระหว่างที่กำลังสวิงปิด ก็ยังทะลุได้อยู่
                }
            }
        }
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

            // เริ่มนับเวลาปิดประตูอัตโนมัติ
            if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = StartCoroutine(AutoCloseDoor());
        }
        else
        {
            if (closeSound != null) audioSource.PlayOneShot(closeSound);

            // ถ้าผู้เล่นกด E ปิดเองก่อนครบ 3 วินาที ให้ยกเลิกการนับเวลาอัตโนมัติ
            if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
        }
    }

    // 3. ระบบนับเวลาปิดประตูอัตโนมัติ
    private IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(autoCloseTime); // รอนับถอยหลังตามเวลาที่ตั้งไว้ (3 วินาที)

        // เมื่อครบเวลา เช็คอีกรอบว่าประตูยังเปิดอยู่ไหม ถ้าเปิดอยู่ให้สั่งปิด
        if (isOpen)
        {
            isOpen = false;
            if (closeSound != null) audioSource.PlayOneShot(closeSound);
        }
    }
}