using UnityEngine;
using System.Collections;

public class FinalChainDoor : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของประตูโซ่ (ตั้งให้ไม่ซ้ำกัน!)")]
    public string doorSaveID = "Chain_Door_1";

    // 🌟 1. ท่าไม้ตาย! ให้ประตูเช็คจากชื่อเซฟของไอเทมตรงๆ เลย
    [Header("🌟 ใส่ชื่อเซฟของคีม (ต้องพิมพ์ให้ตรงกับ creamSaveKey เป๊ะๆ)")]
    public string requiredItemSaveKey = "Item_BoltCutter";

    [Header("ชื่อไอเทมที่จะโชว์ตอนแจ้งเตือนว่าไม่มีของ")]
    public string requiredItemName = "คีมตัดโซ่";

    [Header("การตั้งค่าประตู")]
    public Transform doorBody;
    public float openAngle = 90f;
    public float smoothSpeed = 3f;
    public float autoCloseTime = 3f;

    [Header("ระบบล็อค (ลากโซ่มาใส่)")]
    public GameObject chainLock;

    [Header("เสียง")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;
    public AudioClip cutSound;        // ช่องใส่เสียงตอนโซ่ขาด

    [Header("ระบบฟิสิกส์ (ป้องกันเดินติด)")]
    public Collider solidDoorCollider;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;
    private AudioSource audioSource;
    private Coroutine autoCloseCoroutine;
    private bool isChainCutSaved = false;

    void Start()
    {
        if (doorBody == null) doorBody = transform;
        closedRot = doorBody.localRotation;
        openRot = Quaternion.Euler(0, openAngle, 0) * closedRot;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (PlayerPrefs.GetInt(doorSaveID, 0) == 1)
        {
            isChainCutSaved = true;
            if (chainLock != null) Destroy(chainLock);
        }
    }

    void Update()
    {
        if (chainLock == null && !isChainCutSaved)
        {
            isChainCutSaved = true;
            PlayerPrefs.SetInt(doorSaveID, 1);
            PlayerPrefs.Save();
            Debug.Log("แอบบันทึกเซฟ: โซ่โดนตัดไปแล้ว!");
        }

        Quaternion targetRot = isOpen ? openRot : closedRot;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRot, Time.deltaTime * smoothSpeed);

        if (solidDoorCollider != null)
        {
            if (isOpen)
            {
                solidDoorCollider.isTrigger = true;
            }
            else
            {
                if (Quaternion.Angle(doorBody.localRotation, closedRot) < 2f)
                    solidDoorCollider.isTrigger = false;
                else
                    solidDoorCollider.isTrigger = true;
            }
        }
    }

    public void InteractWithDoor()
    {
        // 1. ถ้ายังมีโซ่ขวางอยู่
        if (chainLock != null)
        {
            // 🌟 2. จุดเปลี่ยนสำคัญ: เช็คเซฟตรงๆ เลยว่ามีค่าเป็น 1 (เคยเก็บคีม) ไหม?
            if (PlayerPrefs.GetInt(requiredItemSaveKey, 0) == 1)
            {
                // มีคีม! สั่งตัดโซ่เลย
                Debug.Log("ความจำเครื่องบอกว่ามีคีม! ตัดโซ่สำเร็จ!");
                if (cutSound != null) audioSource.PlayOneShot(cutSound);
                Destroy(chainLock);
                return;
            }

            // ถ้าไม่มีคีมในระบบเซฟ
            Debug.Log("ประตูล็อค! ติดโซ่ (ยังไม่ได้เก็บคีม)");
            if (lockedSound != null) audioSource.PlayOneShot(lockedSound);

            if (NotificationManager.instance != null)
            {
                NotificationManager.instance.ShowText("ต้องการ " + requiredItemName + " เพื่อตัดโซ่");
            }
            return;
        }

        // 2. ถ้าไม่มีโซ่ -> สลับ เปิด/ปิด
        isOpen = !isOpen;

        if (isOpen)
        {
            if (openSound != null) audioSource.PlayOneShot(openSound);
            if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = StartCoroutine(AutoCloseDoor());
        }
        else
        {
            if (closeSound != null) audioSource.PlayOneShot(closeSound);
            if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
        }
    }

    private IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(autoCloseTime);
        if (isOpen)
        {
            isOpen = false;
            if (closeSound != null) audioSource.PlayOneShot(closeSound);
        }
    }
}