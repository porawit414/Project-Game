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
    public GameObject doorUI;

    [Header("Ending System (UI to Disable)")]
    // 🌟 ลาก Object ที่มีสคริปต์ TutorialManager มาใส่ที่นี่ (เช่น Canvas)
    public TutorialManager tutorialManager; 
    // 🌟 ลาก EvidenceCounter มาใส่ที่นี่
    public GameObject evidenceCounterText; 

    [Header("Ending Effects")]
    public Image fadeToBlackImage; 
    public float fadeSpeed = 0.5f; 
    public GameObject creditsUI; // หน้าจอ THE MYSTERIOUS HOUSE

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
    private Coroutine autoCloseCoroutine;

    void Start()
    {
        AudioListener.volume = 1f;
        if (doorBody == null) doorBody = transform;
        closedRotation = doorBody.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0); 

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        
        if (doorUI != null) doorUI.SetActive(false);
        if (blockingCollider != null) blockingCollider.enabled = true;
        if (fadeToBlackImage != null) fadeToBlackImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isEndingStarted)
        {
            ToggleDoor();
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    public void StartEndingSequence()
    {
        // เช็คจำนวนหลักฐานจาก GameManager
        if (GameManager.instance != null && GameManager.instance.GetEvidenceCount() >= 5)
        {
            if (!isEndingStarted)
            {
                isEndingStarted = true;
                StartCoroutine(FadeToBlackRoutine());
            }
        }
        else
        {
            Debug.Log("หลักฐานยังไม่ครบ 5 ชิ้น!");
        }
    }

    IEnumerator FadeToBlackRoutine()
    {
        if (fadeToBlackImage != null)
        {
            // 1. หยุดการทำงานของตัวละคร
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

            // 🌟 2. สั่งปิดระบบ Tutorial ผ่านฟังก์ชันใหม่ที่เราเพิ่มใน TutorialManager
            if (tutorialManager != null)
            {
                tutorialManager.DisableTutorialSystem();
            }

            // 🌟 3. ปิดตัวนับหลักฐาน 0/5
            if (evidenceCounterText != null) 
            {
                evidenceCounterText.SetActive(false);
            }

            // 4. เริ่มจอดำ
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

            // 5. แสดงหน้าเครดิตตอนจบ
            if (creditsUI != null)
            {
                creditsUI.SetActive(true);
            }
        }
    }

    void ToggleDoor() { if (!isOpen) OpenDoor(); else CloseDoor(); }
    void OpenDoor() { 
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
            if (doorUI != null) doorUI.SetActive(true);
            // ถ้าต้องการให้ชนแล้วจบเลย: StartEndingSequence();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (doorUI != null) doorUI.SetActive(false);
        }
    }
}