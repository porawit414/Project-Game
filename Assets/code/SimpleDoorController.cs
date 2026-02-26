using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimpleDoorController : MonoBehaviour
{
    [Header("Settings")]
    public Transform doorBody;
    public float openAngle = 90f; 
    public float smoothSpeed = 3f;
    public GameObject doorUI;

    [Header("Ending System")]
    public Image fadeToBlackImage; 
    public float fadeSpeed = 0.5f; 

    [Header("Auto Close Settings")]
    public float autoCloseDelay = 3f; 
    private Coroutine autoCloseCoroutine;

    [Header("Auto Close & Physics")]
    public Collider blockingCollider; 

    [Header("Audio Settings")]
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
        // กด E เพื่อ เปิด/ปิด ประตู (จะกดไม่ได้ถ้าเริ่มฉากจบแล้ว)
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isEndingStarted)
        {
            ToggleDoor();
        }

        // การหมุนของประตู
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRotation, Time.deltaTime * smoothSpeed);

        // ระบบฟิสิกส์: ถ้าประตูปิดสนิท ให้เปิด Collider กันคนเดินทะลุ
        float angleRemaining = Quaternion.Angle(doorBody.localRotation, closedRotation);
        if (!isOpen && angleRemaining <= 0.1f)
        {
            if (blockingCollider != null && !blockingCollider.enabled)
                blockingCollider.enabled = true;
        }
    }

    void ToggleDoor()
    {
        if (!isOpen) OpenDoor();
        else CloseDoor();
    }

    void OpenDoor()
    {
        isOpen = true;
        PlaySound(openSound);
        if (blockingCollider != null) blockingCollider.enabled = false;

        // เริ่มนับถอยหลัง 3 วินาทีเพื่อปิดเอง
        if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
        autoCloseCoroutine = StartCoroutine(AutoCloseRoutine());
    }

    void CloseDoor()
    {
        isOpen = false;
        PlaySound(closeSound);
    }

    IEnumerator AutoCloseRoutine()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        if (isOpen) CloseDoor();
    }

    // ฟังก์ชันนี้ถูกเรียกจาก ExitDoorDetector (ตัวดักหน้าประตู)
    public void StartEndingSequence()
    {
        // เช็คว่าเก็บครบ 5 ชิ้นหรือยัง
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
            // === [1] สั่งหยุดทุกอย่าง "ทันที" ตั้งแต่วินาทีแรกที่เริ่มฟังก์ชัน ===
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // 1.1 หยุดการเคลื่อนที่ (Disable CharacterController)
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                // 1.2 ปิดสคริปต์เดิน หมุน ช่องเก็บของ และระบบรับค่าปุ่มกดบนตัวผู้เล่นทั้งหมด
                MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
                foreach (var script in scripts)
                {
                    string sName = script.GetType().Name;
                    if (sName.Contains("Movement") || sName.Contains("Controller") || 
                        sName.Contains("Inventory") || sName.Contains("Input") || 
                        sName.Contains("Interact"))
                    {
                        script.enabled = false;
                    }
                }

                // 1.3 ปิด UI/Canvas ทั้งหมด (รวมถึงตัวเลข 0/5) ทันที
                Canvas[] allCanvases = FindObjectsOfType<Canvas>();
                foreach (Canvas canvas in allCanvases)
                {
                    // ปิดทุก Canvas ยกเว้นอันที่มีรูปจอดำ (Fade)
                    if (canvas != fadeToBlackImage.canvas)
                    {
                        canvas.enabled = false;
                    }
                }

                // 1.4 ล็อกเมาส์ไม่ให้ขยับและซ่อนลูกศร
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // === [2] เมื่อทุกอย่างถูกล็อกแล้ว จึงเริ่มทำให้จอมืดลง ===
            fadeToBlackImage.gameObject.SetActive(true);
            float alpha = 0;
            while (alpha < 1)
            {
                alpha += Time.deltaTime * fadeSpeed;
                fadeToBlackImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            // === [3] เมื่อมืดสนิทแล้ว ค่อยซ่อนโมเดลตัวละคร (เพื่อความชัวร์) ===
            if (player != null)
            {
                Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
                foreach (var r in renderers) r.enabled = false;
            }

            Debug.Log("ฉากจบสมบูรณ์: ผู้เล่นถูกล็อกตั้งแต่เริ่ม Fade และจอดำสนิทแล้ว");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (doorUI != null) doorUI.SetActive(true);
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

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}