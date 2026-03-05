using UnityEngine;
using TMPro;

public class PasswordDoor : MonoBehaviour
{
    [Header("การตั้งค่ารหัสผ่าน")]
    public string correctPassword = "164"; 
    public GameObject keypadUI;      
    public TMP_Text screenText;      

    [Header("การตั้งค่าประตู")]
    public Transform doorHinge;      
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public float autoCloseDelay = 5f;

    [Header("ตัวเล่น (สำคัญ! ลากสคริปต์เดินมาใส่ตรงนี้)")]
    public MonoBehaviour playerMovement; 

    // 🌟 เพิ่มช่องนี้ให้ลาก Collider มาใส่เอง จะได้ไม่บั๊กครับ 🌟
    [Header("ตัวกั้นคน (ลากวัตถุประตูที่มี Collider มาใส่)")]
    public Collider doorCollider; 

    // ตัวแปรเช็คสถานะ
    private bool isLocked = true; 
    private string currentInput = "";
    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        if (doorHinge != null)
        {
            initialRotation = doorHinge.localRotation;
            targetRotation = initialRotation;
        }
        if (keypadUI != null) keypadUI.SetActive(false);
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
            else // ถ้ารหัสผ่านถูกปลดล็อคแล้ว
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

        // --- 3. ระบบหมุนประตูและการชน (เดินทะลุได้ตอนประตูขยับ) ---
        if (doorHinge != null)
        {
            // หมุนประตูอย่างนุ่มนวล
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
            
            // เช็คมุมว่าประตูขยับออกจากจุดเริ่มต้น (ตอนปิด) ไปกี่องศาแล้ว?
            float angleDifference = Quaternion.Angle(doorHinge.localRotation, initialRotation);

            if(doorCollider != null) 
            {
                // ถ้าประตูขยับออกจากจุดเดิมเกิน 1 องศา (กำลังเปิด หรือ กำลังปิด) -> ให้เดินทะลุได้ (isTrigger = true)
                if (angleDifference > 1.0f)
                {
                    doorCollider.isTrigger = true; // ทะลุได้เลย
                }
                // ถ้าประตูกลับมาที่จุดเดิมสนิท (มุมน้อยกว่า 1) -> ให้เดินชนได้ (isTrigger = false)
                else
                {
                    doorCollider.isTrigger = false; // แข็ง ปิดตาย
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
            CloseKeypad();    
            OpenDoor();       
        }
        else
        {
            if(screenText != null) screenText.text = "ERR";
            Invoke("ClearInput", 1f);
        }
    }

    void OpenKeypad()
    {
        if(keypadUI != null) keypadUI.SetActive(true);
        if (playerMovement != null) playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentInput = "";
        UpdateScreen();
    }

    public void CloseKeypad()
    {
        if(keypadUI != null) keypadUI.SetActive(false);
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
        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation;
        Invoke("AutoClose", autoCloseDelay); 
    }

    void CloseDoor()
    {
        isOpen = false;
        targetRotation = initialRotation;
        CancelInvoke("AutoClose"); // ยกเลิกการปิดอัตโนมัติ เพราะผู้เล่นกดปิดเองไปแล้ว
    }

    void AutoClose()
    {
        isOpen = false;
        targetRotation = initialRotation;
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    private void OnTriggerExit(Collider other) 
    { 
        if (other.CompareTag("Player")) 
        {
            isPlayerNear = false;
            if (keypadUI.activeSelf) CloseKeypad();
        }
    }
}