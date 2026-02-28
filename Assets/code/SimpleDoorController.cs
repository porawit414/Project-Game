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
    public GameObject creditsUI; // <--- [เอากลับมาแล้ว] ช่องสำหรับใส่หน้าเครดิต

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
        // [เพิ่มใหม่] รีเซ็ตเสียงของเกมให้กลับมาดัง 100% เสมอตอนเริ่มเกมใหม่
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

                // สั่งหยุดเสียงที่ค้างอยู่บนตัวผู้เล่นทันที
                AudioSource[] playerAudios = player.GetComponentsInChildren<AudioSource>();
                foreach (var audio in playerAudios)
                {
                    audio.Stop();
                }

                Canvas[] allCanvases = FindObjectsOfType<Canvas>();
                foreach (Canvas canvas in allCanvases)
                {
                    if (canvas != fadeToBlackImage.canvas)
                    {
                        canvas.enabled = false;
                    }
                }

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // เตรียมหรี่เสียง
            float startVolume = AudioListener.volume;

            fadeToBlackImage.gameObject.SetActive(true);
            float alpha = 0;
            while (alpha < 1)
            {
                alpha += Time.deltaTime * fadeSpeed;
                fadeToBlackImage.color = new Color(0, 0, 0, alpha);
                
                // ค่อยๆ ลดระดับเสียงหลักของเกมลง
                AudioListener.volume = Mathf.Lerp(startVolume, 0f, alpha);

                yield return null;
            }

            if (player != null)
            {
                Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
                foreach (var r in renderers) r.enabled = false;
            }

            // <--- [เอากลับมาแล้ว] เปิดหน้าเครดิตหลังจากจอดำสนิท --->
            if (creditsUI != null)
            {
                creditsUI.SetActive(true);
            }

            Debug.Log("ฉากจบสมบูรณ์: ผู้เล่นถูกล็อก ภาพมืดสนิท เสียงเงียบ และแสดงเครดิตแล้ว");
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