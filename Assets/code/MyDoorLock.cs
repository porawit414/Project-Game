using UnityEngine; // เรียกใช้งานชุดคำสั่งพื้นฐานของ Unity เพื่อให้ควบคุมสิ่งต่างๆ ในเกมได้
using System.Collections; // เรียกใช้งานชุดคำสั่งพิเศษที่จำเป็นสำหรับการทำระบบหน่วงเวลา (Coroutine)

public class MyDoorLock : MonoBehaviour // สร้างสคริปต์ชื่อ MyDoorLock ให้สามารถนำไปแปะติดกับวัตถุใน Unity ได้
{
    [Header("UI แจ้งเตือน (ลากข้อความมาใส่ตรงนี้)")] // สร้างหัวข้อจัดหมวดหมู่ในหน้าต่าง Inspector ให้ดูเป็นระเบียบ
    public GameObject lockedMessageUI; // ตัวแปรสำหรับเก็บหน้าต่างข้อความ UI ที่จะเด้งเตือนเวลาไม่มีกุญแจ

    [Header("ชื่อกุญแจ")] // สร้างหัวข้อจัดหมวดหมู่ในหน้าต่าง Inspector
    public string keyName = "RoomKey"; // กำหนดชื่อของไอเทมกุญแจที่ต้องใช้เปิดประตูบานนี้ (แก้ชื่อใน Inspector ได้)

    [Header("ผี Jumpscare")] // สร้างหัวข้อจัดหมวดหมู่ในหน้าต่าง Inspector
    public GameObject ghostlyWoman; // ตัวแปรสำหรับเก็บตัวโมเดลผี (Ghostly Woman 3) ที่จะให้โผล่มาหลอก
    public float ghostDelay = 1f; // ตัวแปรสำหรับตั้งเวลาดีเลย์ (วินาที) ก่อนที่ผีจะโผล่มาหลังจากเปิดประตู

    [Header("การตั้งค่าประตู")] // สร้างหัวข้อจัดหมวดหมู่ในหน้าต่าง Inspector
    public Transform doorHinge;  // ตัวแปรสำหรับเก็บจุดหมุน (บานพับ) ของประตู
    public float openAngle = 90f; // องศาที่ประตูจะเปิดออกไป (90 องศาคือเปิดกว้างสุด)
    public float openSpeed = 2f;  // ความเร็วในการสวิงเปิดของบานประตู
    public float autoCloseDelay = 5f; // เวลา (วินาที) ที่จะให้ประตูเปิดค้างไว้ก่อนจะปิดเองอัตโนมัติ

    private bool isOpen = false; // ตัวแปรเก็บสถานะว่า ตอนนี้ประตูเปิดอยู่หรือไม่ (เริ่มต้นคือ false = ปิดอยู่)
    private bool isPlayerNear = false; // ตัวแปรเก็บสถานะว่า ผู้เล่นยืนอยู่ใกล้ประตูหรือไม่ (เริ่มต้นคือ false = ไม่ใกล้)
    private Quaternion targetRotation; // ตัวแปรเก็บค่าองศาการหมุนเป้าหมายที่ประตูจะต้องหมุนไปหา
    private Quaternion initialRotation; // ตัวแปรเก็บค่าองศาการหมุนตอนเริ่มต้น (ตอนประตูปิดสนิท)
    private Coroutine currentCoroutine; // ตัวแปรสำหรับเก็บตัวนับเวลา เพื่อให้สั่งหยุดหรือเริ่มใหม่ได้
    private Collider doorCollider; // ตัวแปรสำหรับเก็บกล่องฟิสิกส์ (Collider) ของประตู เพื่อไว้ตั้งค่าให้เดินทะลุได้

    void Start() // ฟังก์ชันนี้จะทำงานแค่ครั้งเดียว ตอนที่เริ่มเกม
    {
        if(doorHinge != null) // เช็คว่าเราได้ลากบานพับประตูมาใส่ในช่องแล้วหรือยัง (ป้องกันเกมค้าง)
        {
            initialRotation = doorHinge.localRotation; // จดจำองศาเริ่มต้นของประตูเอาไว้
            targetRotation = initialRotation; // ตั้งเป้าหมายการหมุนไว้ที่องศาเริ่มต้น (ให้อยู่นิ่งๆ ไปก่อน)
            doorCollider = doorHinge.GetComponent<Collider>(); // ดึงเอาส่วนประกอบ Collider จากประตูมาเก็บไว้ในตัวแปร
        }
        
        if(lockedMessageUI != null) lockedMessageUI.SetActive(false); // สั่งซ่อนข้อความเตือน "ไม่มีกุญแจ" ไว้ก่อนตอนเริ่มเกม

        if(ghostlyWoman != null) ghostlyWoman.SetActive(false); // สั่งซ่อนตัวผีเอาไว้ก่อนตอนเริ่มเกม จะได้ไม่โผล่มาก่อนเวลา
    }

    void Update() // ฟังก์ชันนี้จะทำงานวนซ้ำๆ ตลอดเวลาทุกเฟรม (ใช้เช็คการกดปุ่มของผู้เล่น)
    {
        // เช็คว่าผู้เล่นอยู่ใกล้ประตู (isPlayerNear) AND กดปุ่ม 'E' AND ประตูยังปิดอยู่ (!isOpen) ใช่หรือไม่?
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isOpen) 
        {
            TryOpenDoor(); // ถ้าเงื่อนไขครบ ให้เรียกใช้ฟังก์ชันพยายามเปิดประตู
        }

        if (doorHinge != null) // เช็คว่ามีบานพับประตูอยู่จริงไหม
        {
            // สั่งให้ประตูหมุนอย่างนุ่มนวล (Slerp) จากมุมปัจจุบัน ไปหามุมเป้าหมาย ตามความเร็วที่กำหนด
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);

            // คำนวณหาความแตกต่างระหว่างมุมปัจจุบัน กับมุมตอนประตูปิดสนิท
            float angleDifference = Quaternion.Angle(doorHinge.localRotation, initialRotation);
            
            // ถ้ามุมห่างกันน้อยกว่า 0.5 องศา (แปลว่าประตูปิดสนิทแล้ว) และสถานะคือไม่ได้เปิดอยู่
            if (angleDifference < 0.5f && !isOpen) 
            {
                // ตั้งค่าประตูให้แข็ง (isTrigger = false) ผู้เล่นเดินชนได้ ไม่ทะลุ
                if(doorCollider != null) doorCollider.isTrigger = false; 
            }
            else // แต่ถ้าประตูกำลังขยับเปิด หรือเปิดอยู่
            {
                // ตั้งค่าประตูให้เป็นวิญญาณ (isTrigger = true) ผู้เล่นเดินทะลุได้ ประตูจะได้ไม่ดันผู้เล่นกระเด็น
                if(doorCollider != null) doorCollider.isTrigger = true; 
            }
        }
    }

    void TryOpenDoor() // ฟังก์ชันสำหรับเช็คว่าเปิดประตูได้ไหม (มีกุญแจไหม)
    {
        // ค้นหาวัตถุในฉากที่มีป้ายชื่อ (Tag) ว่า "Player" (หาตัวผู้เล่น)
        GameObject player = GameObject.FindGameObjectWithTag("Player"); 
        
        if (player != null) // ถ้าหาตัวผู้เล่นเจอ
        {
            // ดึงเอาสคริปต์กระเป๋าเก็บของ (SimpleInventory) ที่ติดอยู่กับตัวผู้เล่นมาใช้งาน
            SimpleInventory inventory = player.GetComponent<SimpleInventory>(); 

            // เช็คว่าผู้เล่นมีกระเป๋า และในกระเป๋ามีไอเทมชื่อตรงกับกุญแจ (keyName) ใช่หรือไม่?
            if (inventory != null && inventory.HasItem(keyName)) 
            {
                OpenDoor(); // ถ้ามีกุญแจ ให้เรียกฟังก์ชันเปิดประตูได้เลย
            }
            else // แต่ถ้าไม่มีกุญแจ
            {
                Debug.Log("ไม่มีกุญแจ!"); // พิมพ์ข้อความแจ้งเตือนในหน้าต่าง Console ของโปรแกรม
                
                if (currentCoroutine != null) StopCoroutine(currentCoroutine); // ถ้ามีระบบโชว์ข้อความทำงานอยู่ ให้หยุดระบบเก่าก่อน
                
                StartCoroutine(ShowLockedMessage()); // เรียกใช้งานระบบโชว์ข้อความ UI บนหน้าจอ
            }
        }
    }

    IEnumerator ShowLockedMessage() // ฟังก์ชันพิเศษสำหรับหน่วงเวลาโชว์ข้อความ
    {
        if (lockedMessageUI != null) // ถ้ามีการใส่หน้าต่างข้อความไว้
        {
            lockedMessageUI.SetActive(true); // สั่งให้ข้อความ UI แสดงขึ้นมาบนหน้าจอ
            yield return new WaitForSeconds(2f); // สั่งให้ระบบหยุดรออยู่ตรงนี้เป็นเวลา 2 วินาที
            lockedMessageUI.SetActive(false); // พอครบ 2 วินาที ก็สั่งให้ซ่อนข้อความ UI กลับไปเหมือนเดิม
        }
    }

    void OpenDoor() // ฟังก์ชันสำหรับสั่งให้ประตูเปิดและเรียกผี
    {
        isOpen = true; // เปลี่ยนสถานะตัวแปรว่าตอนนี้ "ประตูเปิดแล้ว"
        
        // คำนวณองศาเป้าหมายใหม่ โดยให้หมุนไปตามแกน Y (แกนตั้ง) เท่ากับค่าองศาที่ตั้งไว้ (openAngle)
        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation; 
        
        if (ghostlyWoman != null) // ถ้ามีการใส่ตัวผีไว้ในช่อง
        {
            StartCoroutine(ShowGhostDelayed()); // เรียกใช้งานระบบหน่วงเวลาก่อนผีโผล่
        }

        if (currentCoroutine != null) StopCoroutine(currentCoroutine); // ถ้าระบบนับเวลาปิดประตูทำงานอยู่ ให้หยุดของเก่าก่อน
        currentCoroutine = StartCoroutine(AutoCloseRoutine()); // เริ่มต้นนับเวลาถอยหลังเพื่อปิดประตูอัตโนมัติ
    }

    IEnumerator ShowGhostDelayed() // ฟังก์ชันพิเศษสำหรับหน่วงเวลาผีโผล่
    {
        yield return new WaitForSeconds(ghostDelay); // สั่งให้ระบบหยุดรอตามเวลาที่ตั้งไว้ (ghostDelay)
        
        if (ghostlyWoman != null) // ถ้าตัวผียังมีอยู่จริงในฉาก
        {
            ghostlyWoman.SetActive(true); // สั่งให้ผีโผล่ออกมา! (ตื่นขึ้นมาหลอก)
        }
    }

    IEnumerator AutoCloseRoutine() // ฟังก์ชันพิเศษสำหรับนับเวลาถอยหลังปิดประตู
    {
        yield return new WaitForSeconds(autoCloseDelay); // หยุดรอเป็นเวลาเท่ากับตัวเลขที่ตั้งไว้ (autoCloseDelay)
        CloseDoor(); // พอครบเวลา ให้เรียกใช้ฟังก์ชันปิดประตู
    }

    void CloseDoor() // ฟังก์ชันสำหรับสั่งปิดประตู
    {
        isOpen = false; // เปลี่ยนสถานะตัวแปรว่าตอนนี้ "ประตูปิดแล้ว"
        targetRotation = initialRotation; // เปลี่ยนองศาเป้าหมายกลับไปที่องศาเดิมตอนเริ่มต้น (ประตูจะค่อยๆ หมุนกลับเองใน Update)
    }

    private void OnTriggerEnter(Collider other) // ฟังก์ชันนี้จะทำงานเมื่อมีวัตถุเดินเข้ามาในระยะกล่องฟิสิกส์ (Trigger)
    {
        if (other.CompareTag("Player")) // เช็คว่าสิ่งที่เดินเข้ามานั้น มีป้ายชื่อ (Tag) ว่า "Player" หรือไม่
        {
            isPlayerNear = true; // ถ้าเป็นผู้เล่น ให้เปลี่ยนสถานะว่าผู้เล่นอยู่ใกล้ประตูแล้ว
        }
    }

    private void OnTriggerExit(Collider other) // ฟังก์ชันนี้จะทำงานเมื่อมีวัตถุเดินออกจากระยะกล่องฟิสิกส์ (Trigger)
    {
        if (other.CompareTag("Player")) // เช็คว่าสิ่งที่เดินออกไปนั้น มีป้ายชื่อ (Tag) ว่า "Player" หรือไม่
        {
            isPlayerNear = false; // ถ้าเป็นผู้เล่น ให้เปลี่ยนสถานะว่าผู้เล่นไม่ได้อยู่ใกล้ประตูแล้ว
        }
    }
}