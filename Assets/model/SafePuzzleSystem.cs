using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SafePuzzleSystem : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject keypadPanel;      // หน้าจอ UI แผงปุ่มกด
    public GameObject passcodeDisplayObject; // วัตถุที่โชว์ตัวเลขรหัส

    [Header("Safe Settings")]
    public string correctPasscode = "6917"; // รหัสผ่านที่ถูกต้อง

    [Header("Safe Parts to Hide (ลากชิ้นส่วนมาใส่ที่นี่)")]
    public GameObject cube001; // ช่องใส่ Cube.001
    public GameObject cube002; // ช่องใส่ Cube.002
    public GameObject cube003; // ช่องใส่ Cube.003
    public GameObject cylinder001; // ช่องใส่ Cylinder.001
    public GameObject cylinder002; // ช่องใส่ Cylinder.002
    public GameObject cylinder003; // ช่องใส่ Cylinder.003
    public GameObject cylinder004; // ช่องใส่ Cylinder.004
    public GameObject plane;       // ช่องใส่ Plane

    [Header("Reward Item")]
    public GameObject evidenceItem; // ของรางวัลในตู้เซฟ

    [Header("Player Settings")]
    public Behaviour playerLookScript; // สคริปต์ล็อคกล้อง
    public Behaviour inventoryScript;  // สคริปต์ล็อคกระเป๋า

    private string currentInput = "";   // ตัวเลขที่กำลังพิมพ์
    private bool isPlayerNear = false;   // เช็คว่าผู้เล่นอยู่ใกล้ไหม
    private bool isSafeOpen = false;     // เช็คว่าเซฟเปิดหรือยัง
    private bool isKeypadActive = false; // เช็คว่า UI เปิดอยู่ไหม

    private Text legacyText;
    private TMP_Text tmpText;

    void Start()
    {
        // ค้นหาคอมโพเนนต์ Text สำหรับโชว์ตัวเลข
        if (passcodeDisplayObject != null)
        {
            legacyText = passcodeDisplayObject.GetComponent<Text>();
            tmpText = passcodeDisplayObject.GetComponent<TMP_Text>();
        }

        if (keypadPanel != null) keypadPanel.SetActive(false); // ปิด UI ตอนเริ่ม
        if (evidenceItem != null) evidenceItem.SetActive(false); // ซ่อนของรางวัลตอนเริ่ม
        
        UpdateDisplay(); // ล้างหน้าจอโชว์ตัวเลข
    }

    void Update()
    {
        // ตรวจสอบการกดปุ่ม E เพื่อเปิดหรือปิดหน้าจอใส่รหัส
        if (isPlayerNear && !isSafeOpen && Input.GetKeyDown(KeyCode.E))
        {
            if (isKeypadActive) CloseKeypad();
            else OpenKeypad();
            return;
        }

        // ถ้าหน้าจอเปิดอยู่ ให้รับค่าจากคีย์บอร์ด
        if (isKeypadActive)
        {
            HandleKeyboardInput();
        }
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // กด Esc เพื่อปิด
        {
            CloseKeypad();
            return;
        }

        foreach (char c in Input.inputString)
        {
            if (c == '\b') // กดลบตัวเลข
            {
                if (currentInput.Length > 0)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                    UpdateDisplay();
                }
            }
            else if ((c == '\n') || (c == '\r')) // กด Enter เพื่อตรวจรหัส
            {
                CheckPasscode();
            }
            else if (char.IsDigit(c)) // รับเฉพาะตัวเลข 0-9
            {
                if (currentInput.Length < correctPasscode.Length) 
                {
                    currentInput += c;
                    UpdateDisplay();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true; // ผู้เล่นเข้ามาใกล้
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false; // ผู้เล่นเดินออกไป
            CloseKeypad(); // ปิดหน้าจอใส่รหัสทันที
        }
    }

    public void OpenKeypad()
    {
        isKeypadActive = true;
        keypadPanel.SetActive(true);
        currentInput = ""; 
        UpdateDisplay();
        
        Cursor.lockState = CursorLockMode.None; // ปลดล็อกเมาส์
        Cursor.visible = true; // โชว์ตัวชี้เมาส์
        Time.timeScale = 0f; // หยุดเวลาในเกม

        if (playerLookScript != null) playerLookScript.enabled = false; // ล็อคกล้อง
        if (inventoryScript != null) inventoryScript.enabled = false;   // ล็อคกระเป๋า
    }

    public void CloseKeypad()
    {
        isKeypadActive = false;
        keypadPanel.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked; // ล็อกเมาส์
        Cursor.visible = false; // ซ่อนตัวชี้เมาส์
        Time.timeScale = 1f; // ให้เวลาเดินปกติ

        if (playerLookScript != null) playerLookScript.enabled = true; // ปลดล็อกกล้อง
        if (inventoryScript != null) inventoryScript.enabled = true;   // ปลดล็อกกระเป๋า
    }

    public void CheckPasscode()
    {
        if (currentInput == correctPasscode) // ถ้าใส่รหัสถูก
        {
            Debug.Log("รหัสถูกต้อง!");
            isSafeOpen = true;
            CloseKeypad();
            GiveEvidenceItem(); // โชว์ของรางวัล
            
            // 🌟 สั่งให้ชิ้นส่วนทั้งหมดที่ลากมาใส่ หายไปพร้อมกัน 🌟
            if (cube001 != null) cube001.SetActive(false);
            if (cube002 != null) cube002.SetActive(false);
            if (cube003 != null) cube003.SetActive(false);
            if (cylinder001 != null) cylinder001.SetActive(false);
            if (cylinder002 != null) cylinder002.SetActive(false);
            if (cylinder003 != null) cylinder003.SetActive(false);
            if (cylinder004 != null) cylinder004.SetActive(false);
            if (plane != null) plane.SetActive(false);
        }
        else // ถ้าใส่รหัสผิด
        {
            Debug.Log("รหัสผิด!");
            currentInput = ""; 
            UpdateDisplay(); 
        }
    }

    void UpdateDisplay()
    {
        if (legacyText != null) legacyText.text = currentInput;
        if (tmpText != null) tmpText.text = currentInput;
    }

    void GiveEvidenceItem()
    {
        if (evidenceItem != null)
        {
            evidenceItem.SetActive(true); // เสกของรางวัลให้ปรากฏ
        }
    }
}