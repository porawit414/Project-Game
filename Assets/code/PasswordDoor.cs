using UnityEngine;
using TMPro;

public class PasswordDoor : MonoBehaviour
{
    [Header("การตั้งค่ารหัสผ่าน")]
    public string correctPassword = "379"; 
    public GameObject keypadUI;      
    public TMP_Text screenText;      

    [Header("การตั้งค่าประตู")]
    public Transform doorHinge;      
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public float autoCloseDelay = 5f;

    [Header("ตัวเล่น (สำคัญ! ลากสคริปต์เดินมาใส่ตรงนี้)")]
    public MonoBehaviour playerMovement; 

    // ตัวแปรเช็คสถานะ
    private bool isLocked = true; 
    private string currentInput = "";
    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Collider doorCollider; // ตัวกั้นคน

    void Start()
    {
        if (doorHinge != null)
        {
            initialRotation = doorHinge.localRotation;
            targetRotation = initialRotation;
            
            // หา Collider ที่ติดอยู่กับประตู
            doorCollider = doorHinge.GetComponent<Collider>();
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
            else
            {
                if (!isOpen) OpenDoor();
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

        // --- 3. ระบบหมุนประตูและการชน (แก้ใหม่ตรงนี้) ---
        if (doorHinge != null)
        {
            // หมุนประตู
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
            
            // เช็คมุมว่าประตูเปิดอยู่ไหม?
            float angleDifference = Quaternion.Angle(doorHinge.localRotation, initialRotation);

            if(doorCollider != null) 
            {
                // ถ้ามุมห่างจากตอนปิดเกิน 1 องศา (ไม่ว่าจะเปิด หรือกำลังปิด) -> ให้เดินทะลุได้ (isTrigger = true)
                // ถ้าปิดสนิท (มุมน้อยกว่า 1) -> ให้เดินชน (isTrigger = false)
                if (angleDifference > 1.0f)
                {
                    doorCollider.isTrigger = true; // ทะลุได้
                }
                else
                {
                    doorCollider.isTrigger = false; // แข็ง (ปิดสนิทแล้ว)
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