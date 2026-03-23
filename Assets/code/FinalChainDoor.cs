using UnityEngine;
using System.Collections;

public class FinalChainDoor : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของประตูโซ่")]
    public string doorSaveID = "Chain_Door_1";

    [Header("🌟 ใส่ชื่อเซฟของคีม")]
    public string requiredItemSaveKey = "Item_BoltCutter";

    [Header("ชื่อไอเทมที่จะโชว์ตอนแจ้งเตือน")]
    public string requiredItemName = "คีมตัดโซ่";

    [Header("การตั้งค่าประตู")]
    public Transform doorBody;
    public float openAngle = 90f;
    public float smoothSpeed = 3f;

    [Header("ระบบล็อค (ลากโซ่มาใส่)")]
    public GameObject chainLock;

    [Header("เสียง")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;
    public AudioClip cutSound;

    // 🚨 1. ตั้งค่าระยะห่างที่ปลอดภัยในการปิดประตู
    [Header("ระบบป้องกันกระเด็น (เช็คระยะ)")]
    public float safeDistanceToClose = 1.5f;

    // 🚨 2. ลากตัวละคร (PlayerCapsule) มาใส่ในช่องนี้!
    public Transform playerTransform;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;
    private AudioSource audioSource;
    private bool isChainCutSaved = false;

    void Start()
    {
        if (doorBody == null) doorBody = transform;
        closedRot = doorBody.localRotation;
        openRot = closedRot * Quaternion.Euler(0, openAngle, 0);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (PlayerPrefs.GetInt(doorSaveID, 0) == 1)
        {
            isChainCutSaved = true;
            if (chainLock != null) Destroy(chainLock);
        }

        // หาตัวละครแบบอัตโนมัติ (ถ้าไม่ได้ลากใส่ใน Inspector)
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        if (chainLock == null && !isChainCutSaved)
        {
            isChainCutSaved = true;
            PlayerPrefs.SetInt(doorSaveID, 1);
            PlayerPrefs.Save();
        }

        Quaternion targetRot = isOpen ? openRot : closedRot;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }

    public void InteractWithDoor()
    {
        if (IntroDialog.isIntroActive || SimplePauseMenu.isGamePaused || TutorialManager.isTutorialOpen) return;

        if (chainLock != null)
        {
            if (PlayerPrefs.GetInt(requiredItemSaveKey, 0) == 1)
            {
                if (cutSound != null) audioSource.PlayOneShot(cutSound);
                chainLock.transform.SetParent(null);
                Rigidbody rb = chainLock.GetComponent<Rigidbody>();
                if (rb == null) rb = chainLock.AddComponent<Rigidbody>();
                rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
                Destroy(chainLock, 3f);
                chainLock = null;
                return;
            }

            if (lockedSound != null) audioSource.PlayOneShot(lockedSound);
            if (NotificationManager.instance != null)
                NotificationManager.instance.ShowText("ต้องการ " + requiredItemName + " เพื่อตัดโซ่");
            return;
        }

        // 🚨 ไม้กั้นระบบกระเด็น: ถ้าประตูเปิดอยู่ และเราจะสั่ง "ปิด" ให้เช็คระยะก่อน!
        if (isOpen && playerTransform != null)
        {
            // เช็คว่าตัวละครอยู่ห่างจากตัวประตูแค่ไหน
            float distanceToPlayer = Vector3.Distance(doorBody.position, playerTransform.position);

            // ถ้าอยู่ใกล้เกินไป (ขวางประตูอยู่) -> ไม่ให้ปิด!
            if (distanceToPlayer < safeDistanceToClose)
            {
                Debug.Log("ยืนขวางประตูอยู่ ปิดไม่ได้!");
                // (ถ้ามีระบบโชว์ข้อความบนจอ ก็เอามาใส่ตรงนี้ได้เลย)
                if (NotificationManager.instance != null)
                {
                    NotificationManager.instance.ShowText("ยืนขวางประตูอยู่ ปิดไม่ได้!");
                }
                return; // เด้งออก ไม่ยอมให้เปิด/ปิด
            }
        }

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