using UnityEngine;
using System.Collections;

public class MyDoorLock : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของประตูนี้ (ถ้ามีหลายประตู ต้องตั้งไม่ให้ซ้ำกัน)")]
    public string doorSaveID = "Unlocked_Door_1";

    [Header("UI แจ้งเตือน (ลากข้อความมาใส่ตรงนี้)")]
    public GameObject lockedMessageUI;

    [Header("ชื่อกุญแจ")]
    public string keyName = "RoomKey";

    [Header("ผี Jumpscare")]
    public GameObject ghostlyWoman;
    public float ghostDelay = 1f;

    [Header("การตั้งค่าประตู")]
    public Transform doorHinge;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public float autoCloseDelay = 5f;

    // 🌟 --- ระบบเสียงประตูที่เพิ่มเข้ามาใหม่ --- 🌟
    [Header("ระบบเสียงประตู")]
    public AudioSource doorAudioSource; // ลากตัวปล่อยเสียง (Audio Source) มาใส่
    public AudioClip openSound;         // ลากไฟล์เสียง "เปิดประตู" มาใส่
    public AudioClip closeSound;        // ลากไฟล์เสียง "ปิดประตู" มาใส่

    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private Coroutine currentCoroutine;
    private Collider doorCollider;

    // ตัวแปรจำว่าประตูปลดล็อคถาวรแล้ว
    private bool isPermanentlyUnlocked = false;

    void Start()
    {
        if (doorHinge != null)
        {
            initialRotation = doorHinge.localRotation;
            targetRotation = initialRotation;
            doorCollider = doorHinge.GetComponent<Collider>();
        }

        if (lockedMessageUI != null) lockedMessageUI.SetActive(false);
        if (ghostlyWoman != null) ghostlyWoman.SetActive(false);

        // พยายามหา AudioSource อัตโนมัติถ้าลืมลากใส่ช่อง
        if (doorAudioSource == null)
        {
            doorAudioSource = GetComponent<AudioSource>();
        }

        // เช็คตอนเริ่มเกมว่าประตูนี้เคยไขแล้วหรือยัง?
        if (PlayerPrefs.GetInt(doorSaveID, 0) == 1)
        {
            isPermanentlyUnlocked = true;
            Debug.Log("ประตู " + doorSaveID + " เคยถูกไขแล้ว! วันนี้เปิดได้ฟรีๆ");
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen)
            {
                TryOpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }

        if (doorHinge != null)
        {
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
            float angleDifference = Quaternion.Angle(doorHinge.localRotation, initialRotation);

            if (angleDifference < 0.5f && !isOpen)
            {
                if (doorCollider != null) doorCollider.isTrigger = false;
            }
            else
            {
                if (doorCollider != null) doorCollider.isTrigger = true;
            }
        }
    }

    void TryOpenDoor()
    {
        if (isPermanentlyUnlocked)
        {
            OpenDoor();
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var inventory = player.GetComponent<SimpleInventory>();
            if (inventory != null && inventory.HasItem(keyName))
            {
                isPermanentlyUnlocked = true;
                PlayerPrefs.SetInt(doorSaveID, 1);
                PlayerPrefs.Save();

                Debug.Log("ไขประตู " + doorSaveID + " ด้วยกุญแจสำเร็จ! ระบบจำไว้แล้ว");

                OpenDoor();
            }
            else
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(ShowLockedMessage());
            }
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation;

        // 🌟 เล่นเสียงเปิดประตู
        if (doorAudioSource != null && openSound != null)
        {
            doorAudioSource.PlayOneShot(openSound);
        }

        if (ghostlyWoman != null) StartCoroutine(ShowGhostDelayed());

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(AutoCloseRoutine());
    }

    public void CloseDoor()
    {
        isOpen = false;
        targetRotation = initialRotation;

        // 🌟 เล่นเสียงปิดประตู
        if (doorAudioSource != null && closeSound != null)
        {
            doorAudioSource.PlayOneShot(closeSound);
        }

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
    }

    IEnumerator ShowLockedMessage()
    {
        if (lockedMessageUI != null)
        {
            lockedMessageUI.SetActive(true);
            yield return new WaitForSeconds(2f);
            lockedMessageUI.SetActive(false);
        }
    }

    IEnumerator ShowGhostDelayed()
    {
        yield return new WaitForSeconds(ghostDelay);
        if (ghostlyWoman != null) ghostlyWoman.SetActive(true);
    }

    IEnumerator AutoCloseRoutine()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        if (isOpen) CloseDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = false;
    }
}