using UnityEngine;

using TMPro;



public class PasswordDoor : MonoBehaviour

{

    [Header("การตั้งค่ารหัสผ่าน")]

    public string correctPassword = "379"; // ตั้งรหัสเป็น 379 ตามที่ขอ

    public GameObject keypadUI;      

    public TMP_Text screenText;      



    [Header("การตั้งค่าประตู")]

    public Transform doorHinge;      

    public float openAngle = 90f;

    public float openSpeed = 2f;

    public float autoCloseDelay = 5f;



    // ตัวแปรเช็คสถานะ

    private bool isLocked = true; // เริ่มเกมมาประตูจะล็อคอยู่

    private string currentInput = "";

    private bool isOpen = false;

    private bool isPlayerNear = false;

    private Quaternion initialRotation;

    private Quaternion targetRotation;

    private Collider doorCollider;



    void Start()

    {

        if (doorHinge != null)

        {

            initialRotation = doorHinge.localRotation;

            targetRotation = initialRotation;

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

                // ถ้ายังล็อคอยู่ -> ให้เปิด/ปิด แผงกดรหัส

                if (keypadUI.activeSelf) CloseKeypad();

                else OpenKeypad();

            }

            else

            {

                // ถ้าปลดล็อคแล้ว -> ให้เปิดประตูเลย (ไม่ต้องโชว์แผงรหัส)

                if (!isOpen) OpenDoor();

            }

        }



        // --- 2. ระบบพิมพ์รหัส (ทำงานเฉพาะตอนประตูล็อค และเปิดแผงอยู่) ---

        if (isLocked && keypadUI != null && keypadUI.activeSelf)

        {

            // รับค่าตัวเลขจากคีย์บอร์ด

            foreach (char c in Input.inputString)

            {

                if (char.IsDigit(c)) // ถ้ากดตัวเลข

                {

                    InputNumber(c.ToString());

                }

            }



            // กด Backspace เพื่อลบ

            if (Input.GetKeyDown(KeyCode.Backspace))

            {

                currentInput = "";

                UpdateScreen();

            }

        }



        // --- 3. ระบบหมุนประตู ---

        if (doorHinge != null)

        {

            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);

           

            // ทำให้เดินทะลุได้ตอนประตูเปิด

            float angle = Quaternion.Angle(doorHinge.localRotation, initialRotation);

            if(doorCollider != null) doorCollider.isTrigger = (angle >= 0.5f || isOpen);

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

            // รหัสถูกต้อง!

            isLocked = false; // ปลดล็อคถาวร!!

            CloseKeypad();    // ปิดแผง

            OpenDoor();       // เปิดประตู

        }

        else

        {

            // รหัสผิด

            if(screenText != null) screenText.text = "ERR";

            Invoke("ClearInput", 1f);

        }

    }



    void OpenKeypad()

    {

        if(keypadUI != null) keypadUI.SetActive(true);

        currentInput = "";

        UpdateScreen();

    }



    public void CloseKeypad()

    {

        if(keypadUI != null) keypadUI.SetActive(false);

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

        Invoke("AutoClose", autoCloseDelay); // สั่งปิดเองตามเวลา

    }



    void AutoClose()
    
    {

        isOpen = false;

        targetRotation = initialRotation;

    }



    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }

    private void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) isPlayerNear = false; }

}