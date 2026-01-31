using UnityEngine;

using System.Collections; // บรรทัดนี้สำคัญมาก!



public class MyDoorLock : MonoBehaviour

{

    [Header("UI แจ้งเตือน (ลากข้อความมาใส่ตรงนี้)")]

    public GameObject lockedMessageUI; // <--- ช่องใหม่ที่จะโผล่มา



    [Header("ชื่อกุญแจ")]

    public string keyName = "RoomKey";



    [Header("การตั้งค่าประตู")]

    public Transform doorHinge;  

    public float openAngle = 90f;

    public float openSpeed = 2f;  

    public float autoCloseDelay = 5f;



    private bool isOpen = false;

    private bool isPlayerNear = false;

    private Quaternion targetRotation;

    private Quaternion initialRotation;

    private Coroutine currentCoroutine;

    private Collider doorCollider;



    void Start()

    {

        if(doorHinge != null)

        {

            initialRotation = doorHinge.localRotation;

            targetRotation = initialRotation;

            doorCollider = doorHinge.GetComponent<Collider>();

        }

       

        // สั่งปิดข้อความแจ้งเตือนตอนเริ่มเกม (กันเหนียว)

        if(lockedMessageUI != null) lockedMessageUI.SetActive(false);

    }



    void Update()

    {

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isOpen)

        {

            TryOpenDoor();

        }



        // --- ส่วนจัดการการหมุนและระบบฟิสิกส์ (ประตูวิญญาณ) ---

        if (doorHinge != null)

        {

            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);



            float angleDifference = Quaternion.Angle(doorHinge.localRotation, initialRotation);

           

            // ถ้าปิดสนิท -> แข็ง (ชนได้)

            if (angleDifference < 0.5f && !isOpen)

            {

                if(doorCollider != null) doorCollider.isTrigger = false;

            }

            // ถ้ากำลังขยับ -> เป็นวิญญาณ (เดินทะลุได้ ไม่ดันผู้เล่น)

            else

            {

                if(doorCollider != null) doorCollider.isTrigger = true;

            }

        }

    }



    void TryOpenDoor()

    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)

        {

            SimpleInventory inventory = player.GetComponent<SimpleInventory>();



            // เช็คว่ามีกุญแจไหม?

            if (inventory != null && inventory.HasItem(keyName))

            {

                OpenDoor(); // มีกุญแจ -> เปิด

            }

            else

            {

                // ไม่มีกุญแจ -> โชว์ข้อความแจ้งเตือน

                Debug.Log("ไม่มีกุญแจ!");

                if (currentCoroutine != null) StopCoroutine(currentCoroutine); // รีเซ็ตเวลานับถอยหลังเก่า (ถ้ามี)

                StartCoroutine(ShowLockedMessage());

            }

        }

    }



    // ฟังก์ชันโชว์ข้อความ 2 วินาที แล้วหายไป

    IEnumerator ShowLockedMessage()

    {

        if (lockedMessageUI != null)

        {

            lockedMessageUI.SetActive(true); // โชว์ข้อความ

            yield return new WaitForSeconds(2f); // รอ 2 วิ

            lockedMessageUI.SetActive(false); // ซ่อนข้อความ

        }

    }



    void OpenDoor()

    {

        isOpen = true;

        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation;

       

        // เริ่มนับถอยหลังปิดประตู

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(AutoCloseRoutine());

    }



    IEnumerator AutoCloseRoutine()

    {

        yield return new WaitForSeconds(autoCloseDelay);

        CloseDoor();

    }



    void CloseDoor()

    {

        isOpen = false;

        targetRotation = initialRotation;

    }



    private void OnTriggerEnter(Collider other)

    {

        if (other.CompareTag("Player")) isPlayerNear = true;

    }



    private void OnTriggerExit(Collider other)

    {

        if (other.CompareTag("Player")) isPlayerNear = false;

    }

}