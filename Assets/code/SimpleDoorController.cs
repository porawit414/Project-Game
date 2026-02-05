using UnityEngine;

public class SimpleDoorController : MonoBehaviour
{
    [Header("Settings")]
    public Transform doorBody;    
    public float openAngle = 90f; 
    public float smoothSpeed = 3f; 
    public GameObject doorUI; // <--- เพิ่มช่องสำหรับลาก "DoorPrompt" มาใส่

    [Header("Audio Settings")]
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private bool isPlayerNearby = false; 
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private AudioSource audioSource;

    void Start()
    {
        if (doorBody == null) doorBody = transform;

        closedRotation = doorBody.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;

        // เริ่มเกมมา ให้ซ่อน UI ไว้ก่อนเผื่อลืมปิดในหน้า Inspector
        if (doorUI != null) doorUI.SetActive(false);
    }

    void Update()
    {
        // เช็คการกดปุ่ม E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; 
            
            if (isOpen) PlaySound(openSound);
            else PlaySound(closeSound);
        }

        // ส่วนหมุนประตู
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        doorBody.localRotation = Quaternion.Slerp(doorBody.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    // เมื่อเดินเข้าเขต ให้แสดงคำแจ้งเตือน
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (doorUI != null) doorUI.SetActive(true); // <--- สั่งให้คำแจ้งเตือนปรากฏขึ้น
        }
    }

    // เมื่อเดินออกเขต ให้ซ่อนคำแจ้งเตือน
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (doorUI != null) doorUI.SetActive(false); // <--- สั่งให้คำแจ้งเตือนหายไป
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.Stop(); 
            audioSource.PlayOneShot(clip);
        }
    }
}