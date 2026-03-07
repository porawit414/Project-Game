using UnityEngine;
using TMPro;

public class PasswordDoor : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของประตูรหัส (ตั้งให้ไม่ซ้ำกัน!)")]
    public string doorSaveID = "Password_Door_1";

    [Header("การตั้งค่ารหัสผ่าน")]
    public string correctPassword = "164";
    public GameObject keypadUI;
    public TMP_Text screenText;

    [Header("การตั้งค่าประตู")]
    public Transform doorHinge;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public float autoCloseDelay = 5f;

    // 🌟 --- เพิ่มช่องใส่เสียงตรงนี้ --- 🌟
    [Header("ระบบเสียง (ลากไฟล์เสียงมาใส่)")]
    public AudioClip openDoorSound;    // เสียงตอนประตูเปิด
    public AudioClip closeDoorSound;   // เสียงตอนประตูปิด
    public AudioClip accessGrantedSound; // เสียงติ๊ดดด! (รหัสถูก)
    public AudioClip accessDeniedSound;  // เสียงตู๊ดดด! (รหัสผิด)

    [Header("ตัวเล่น (สำคัญ! ลากสคริปต์เดินมาใส่ตรงนี้)")]
    public MonoBehaviour playerMovement;

    [Header("ตัวกั้นคน (ลากวัตถุประตูที่มี Collider มาใส่)")]
    public Collider doorCollider;

    // ตัวแปรเช็คสถานะ
    private bool isLocked = true;
    private string currentInput = "";
    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    // ตัวเล่นเสียง
    private AudioSource audioSource;

    void Start()
    {
        // สร้างระบบเสียงให้อัตโนมัติ ไม่ต้องสร้างเองใน Inspector
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // ให้เสียงเป็น 3 มิติ (ดังจากที่ตั้งของประตู)

        if (doorHinge != null)
        {
            initialRotation = doorHinge.localRotation;
            targetRotation = initialRotation;
        }
        if (keypadUI != null) keypadUI.SetActive(false);

        // เช็คความจำตอนโหลดฉาก
        if (PlayerPrefs.GetInt(doorSaveID, 0) == 1)
        {
            isLocked = false;
            Debug.Log("ประตูรหัส " + doorSaveID + " เคยถูกปลดล็อคแล้ว วันนี้เปิดได้เลย!");
        }
    }

    void Update()
    {
        // --- 1. การกดปุ่ม E เพื่อสั่งงาน ---
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (isLocked)
            {
                if (keypadUI.activeSelf) CloseKeypad();
                else OpenKeypad();
            }
            else // ถ้ารหัสถูกปลดล็อคแล้ว
            {
                if (isOpen)
                {
                    CloseDoor(); // ถ้าเปิดอยู่ กด E ให้ปิด
                }
                else
                {
                    OpenDoor();  // ถ้าปิดอยู่ กด E ให้เปิด
                }
            }
        }

        // --- 2. ระบบพิมพ์รหัส ---
        if (isLocked && keypadUI != null && keypadUI.activeSelf)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c)) InputNumber(c.ToString());
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                currentInput = "";
                UpdateScreen();
            }
        }

        // --- 3. ระบบหมุนประตูและการชน ---
        if (doorHinge != null)
        {
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
            float angleDifference = Quaternion.Angle(doorHinge.localRotation, initialRotation);

            if (doorCollider != null)
            {
                if (angleDifference > 1.0f)
                {
                    doorCollider.isTrigger = true;
                }
                else
                {
                    doorCollider.isTrigger = false;
                }
            }
        }
    }

    void InputNumber(string number)
    {
        if (currentInput.Length < 3)
        {
            currentInput += number;
            UpdateScreen();
        }

        if (currentInput.Length == 3)
        {
            CheckPassword();
        }
    }

    void CheckPassword()
    {
        if (currentInput == correctPassword)
        {
            isLocked = false;

            // 🌟 เล่นเสียงติ๊ด! รหัสถูก
            if (accessGrantedSound != null) audioSource.PlayOneShot(accessGrantedSound);

            PlayerPrefs.SetInt(doorSaveID, 1);
            PlayerPrefs.Save();

            CloseKeypad();

            // ให้รอฟังเสียงติ๊ดก่อนครึ่งวินาที ค่อยเปิดประตู
            Invoke("OpenDoor", 0.5f);
        }
        else
        {
            // 🌟 เล่นเสียงตู๊ด! รหัสผิด
            if (accessDeniedSound != null) audioSource.PlayOneShot(accessDeniedSound);

            if (screenText != null) screenText.text = "ERR";
            Invoke("ClearInput", 1f);
        }
    }

    void OpenKeypad()
    {
        if (keypadUI != null) keypadUI.SetActive(true);
        if (playerMovement != null) playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentInput = "";
        UpdateScreen();
    }

    public void CloseKeypad()
    {
        if (keypadUI != null) keypadUI.SetActive(false);
        if (playerMovement != null) playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentInput = "";
    }

    void ClearInput()
    {
        currentInput = "";
        UpdateScreen();
    }

    void UpdateScreen()
    {
        if (screenText != null) screenText.text = currentInput;
    }

    void OpenDoor()
    {
        isOpen = true;

        // 🌟 เล่นเสียงเปิดประตู
        if (openDoorSound != null) audioSource.PlayOneShot(openDoorSound);

        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation;
        Invoke("AutoClose", autoCloseDelay);
    }

    void CloseDoor()
    {
        isOpen = false;

        // 🌟 เล่นเสียงปิดประตู
        if (closeDoorSound != null) audioSource.PlayOneShot(closeDoorSound);

        targetRotation = initialRotation;
        CancelInvoke("AutoClose");
    }

    void AutoClose()
    {
        isOpen = false;

        // 🌟 เล่นเสียงปิดประตู (ตอนมันปิดเองอัตโนมัติ)
        if (closeDoorSound != null) audioSource.PlayOneShot(closeDoorSound);

        targetRotation = initialRotation;
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (keypadUI != null && keypadUI.activeSelf) CloseKeypad();
        }
    }
}