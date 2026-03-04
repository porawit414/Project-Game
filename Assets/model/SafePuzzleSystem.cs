using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SafePuzzleSystem : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("ลาก KeypadPanel มาใส่ตรงนี้")]
    public GameObject keypadPanel;      
    
    [Tooltip("ลากตัวหนังสือ PasscodeDisplay (Text) มาใส่ตรงนี้")]
    public GameObject passcodeDisplayObject; 

    [Header("Safe Settings")]
    [Tooltip("รหัสผ่านสำหรับปลดล็อก (ตอนนี้ตั้งไว้เป็น 999)")]
    public string correctPasscode = "999"; 

    [Header("Reward Item")]
    [Tooltip("ลากไอเทม NumberNote_Evidence มาใส่ตรงนี้ (มันจะโผล่มาเมื่อรหัสถูก)")]
    public GameObject evidenceItem; 

    private string currentInput = "";
    private bool isPlayerNear = false;
    private bool isSafeOpen = false;
    private bool isKeypadActive = false;

    private Text legacyText;
    private TMP_Text tmpText;

    void Start()
    {
        // ตรวจสอบชนิดของตัวหนังสืออัตโนมัติ
        if (passcodeDisplayObject != null)
        {
            legacyText = passcodeDisplayObject.GetComponent<Text>();
            tmpText = passcodeDisplayObject.GetComponent<TMP_Text>();
        }

        // ปิดหน้าจอ UI ไว้ก่อนตอนเริ่มเกม
        if (keypadPanel != null) keypadPanel.SetActive(false);
        
        // ซ่อนไอเทมหลักฐานไว้ก่อนตอนเริ่มเกม (จะโผล่มาเมื่อรหัสถูก)
        if (evidenceItem != null) evidenceItem.SetActive(false);
        
        UpdateDisplay();
    }

    void Update()
    {
        // 1. กด E เพื่อเปิดหน้าจอ (เมื่ออยู่ใกล้และเซฟยังไม่เปิด)
        if (isPlayerNear && !isSafeOpen && !isKeypadActive && Input.GetKeyDown(KeyCode.E))
        {
            OpenKeypad();
            return; 
        }

        // 2. ถ้าหน้าจอเปิดอยู่ ให้รับค่าจากการพิมพ์คีย์บอร์ด
        if (isKeypadActive)
        {
            HandleKeyboardInput();
        }
    }

    void HandleKeyboardInput()
    {
        // กดปุ่ม Esc เพื่อปิดหน้าจอ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
            return;
        }

        // อ่านค่าที่ผู้เล่นพิมพ์เข้ามาผ่านคีย์บอร์ด
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // กด Backspace เพื่อลบตัวเลข
            {
                if (currentInput.Length > 0)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    UpdateDisplay();
                }
            }
            else if ((c == '\n') || (c == '\r')) // กด Enter เพื่อยืนยันรหัส
            {
                CheckPasscode();
            }
            else if (char.IsDigit(c)) // ถ้ากดตัวเลข 0-9
            {
                // พิมพ์ได้ไม่เกินจำนวนหลักของรหัสที่ตั้งไว้ (3 หลัก)
                if (currentInput.Length < correctPasscode.Length) 
                {
                    currentInput += c;
                    UpdateDisplay();
                }
            }
        }
    }

    // เช็คว่าผู้เล่นเดินมาเข้าใกล้ตู้เซฟ (ต้องมี Box Collider แบบ Is Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    // เช็คว่าผู้เล่นเดินออกจากระยะตู้เซฟ
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            CloseKeypad(); 
        }
    }

    public void OpenKeypad()
    {
        isKeypadActive = true;
        keypadPanel.SetActive(true);
        currentInput = ""; // ล้างหน้าจอทุกครั้งที่เริ่มกดใหม่
        UpdateDisplay();
        
        // ปลดล็อกเมาส์ (ถ้าจำเป็นต้องใช้เมาส์คลิกส่วนอื่น)
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void CloseKeypad()
    {
        isKeypadActive = false;
        keypadPanel.SetActive(false);
        
        // ล็อกเมาส์กลับไปที่ตัวละคร
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    public void CheckPasscode()
    {
        if (currentInput == correctPasscode)
        {
            Debug.Log("รหัสถูกต้อง! หลักฐานปรากฏขึ้น");
            isSafeOpen = true;
            CloseKeypad();
            GiveEvidenceItem(); 
        }
        else
        {
            Debug.Log("รหัสผิด! ลองใหม่อีกครั้ง");
            currentInput = ""; // ล้างรหัสที่ผิดออก
            UpdateDisplay(); 
        }
    }

    void UpdateDisplay()
    {
        // อัปเดตตัวเลขลงบนหน้าจอ UI
        if (legacyText != null) legacyText.text = currentInput;
        if (tmpText != null) tmpText.text = currentInput;
    }

    void GiveEvidenceItem()
    {
        // สั่งให้ไอเทมหลักฐาน NumberNote_Evidence ปรากฏขึ้นมา
        if (evidenceItem != null)
        {
            evidenceItem.SetActive(true);
        }
    }
}