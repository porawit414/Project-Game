using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SimpleDoorController : MonoBehaviour
{
    [Header("Settings")]
    public Transform doorBody;
    public float openAngle = 90f;
    public float smoothSpeed = 3f;

    // 🌟 เปลี่ยนจากช่องเดียว เป็น 2 ช่อง สำหรับข้อความเปิดและปิด
    [Header("UI ข้อความประตู")]
    public GameObject openDoorUI;  // ลากข้อความ "เปิดประตู" มาใส่
    public GameObject closeDoorUI; // ลากข้อความ "ปิดประตู" มาใส่

    [Header("Ending System (UI to Disable)")]
    public TutorialManager tutorialManager;
    public GameObject evidenceCounterText;

    [Header("Ending Effects")]
    public Image fadeToBlackImage;
    public float fadeSpeed = 0.5f;
    public GameObject creditsUI;

    [Header("Auto Close & Audio")]
    public float autoCloseDelay = 3f;
    public Collider blockingCollider;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private bool isPlayerNearby = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private AudioSource audioSource;
    private bool isEndingStarted = false;

    void Start()
    {
        AudioListener.volume = 1f;
        if (doorBody == null) doorBody = transform;
        closedRotation = doorBody.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // ซ่อนข้อความทั้งคู่ตอนเริ่มเกม
        if (openDoorUI != null) openDoorUI.SetActive(false);
        if (closeDoorUI != null) closeDoorUI.SetActive(false);

        if (blockingCollider != null) blockingCollider.enabled = true;
        if (fadeToBlackImage != null) fadeToBlackImage.gameObject.SetActive(false);
    }

    void Update()
    {
        // 🌟 1. เช็คการกด E เพื่อเปิด/ปิด ประตู
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isEndingStarted)
        {
            ToggleDoor();
        }

        // 🌟 2. ระบบสลับโชว์ข้อความอัตโนมัติ
        if (isPlayerNearby && !isEndingStarted)
        {
            if (!isOpen)
            {
                // ถ้าประตูปิดอยู่ -> โชว์คำว่า "เปิด" / ซ่อนคำว่า "ปิด"
                if (openDoorUI != null) openDoorUI.SetActive(true);
                if (closeDoorUI != null) closeDoorUI.SetActive(false);
            }
            else
            {
                // ถ้าประตูเปิดอยู่ -> โชว์คำว่า "ปิด" / ซ่อนคำว่า "เปิด"
                if (openDoorUI != null) openDoorUI.SetActive(false);
                if (closeDoorUI != null) closeDoorUI.SetActive(true);
            }
        }
        else
        {
            // ถ้าเดินออกห่างจากประตู -> ซ่อนทั้งคู่
            if (openDoorUI != null) openDoorUI.SetActive(false);
            if (closeDoorUI != null) closeDoorUI.SetActive(false);
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    public void StartEndingSequence()
    {
        if (GameManager.instance != null && GameManager.instance.GetEvidenceCount() >= 5)
        {
            if (!isEndingStarted)
            {
                isEndingStarted = true;
                StartCoroutine(FadeToBlackRoutine());
            }
        }
    }

    IEnumerator FadeToBlackRoutine()
    {
        if (fadeToBlackImage != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                foreach (var script in player.GetComponents<MonoBehaviour>())
                {
                    if (script.GetType().Name.Contains("Movement") || script.GetType().Name.Contains("Controller"))
                        script.enabled = false;
                }

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (tutorialManager != null) tutorialManager.DisableTutorialSystem();
            if (evidenceCounterText != null) evidenceCounterText.SetActive(false);

            float startVolume = AudioListener.volume;
            fadeToBlackImage.gameObject.SetActive(true);
            float alpha = 0;
            while (alpha < 1)
            {
                alpha += Time.deltaTime * fadeSpeed;
                fadeToBlackImage.color = new Color(0, 0, 0, alpha);
                AudioListener.volume = Mathf.Lerp(startVolume, 0f, alpha);
                yield return null;
            }

            if (creditsUI != null) creditsUI.SetActive(true);
        }
    }

    void ToggleDoor() { if (!isOpen) OpenDoor(); else CloseDoor(); }
    void OpenDoor()
    {
        isOpen = true; PlaySound(openSound);
        if (blockingCollider != null) blockingCollider.enabled = false;
    }
    void CloseDoor() { isOpen = false; PlaySound(closeSound); }
    void PlaySound(AudioClip clip) { if (clip != null && audioSource != null) audioSource.PlayOneShot(clip); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}