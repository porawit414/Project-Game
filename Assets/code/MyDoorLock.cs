using UnityEngine;

public class MyDoorLock : MonoBehaviour
{
    [Header("ชื่อกุญแจ (ต้องตรงกับ Item Name ในกุญแจ)")]
    public string keyName = "RoomKey"; 

    [Header("การตั้งค่าประตู")]
    public Transform doorHinge;   // ตัวบานประตูที่หมุนได้ (Interior_Door)
    public float openAngle = 90f; // องศาที่จะเปิด
    public float openSpeed = 2f;  // ความเร็ว

    private bool isOpen = false;
    private bool isPlayerNear = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;

    void Start()
    {
        // จำมุมเริ่มต้นไว้
        if(doorHinge != null)
        {
            initialRotation = doorHinge.localRotation;
            targetRotation = initialRotation;
        }
    }

    void Update()
    {
        // เงื่อนไข: อยู่ใกล้ + กด E + ประตูยังไม่เปิด
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            TryOpenDoor();
        }

        // สั่งหมุนประตู (Animation)
        if (doorHinge != null)
        {
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
        }
    }

    void TryOpenDoor()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // เรียกใช้กระเป๋าที่เราเพิ่งแก้ไป
            SimpleInventory inventory = player.GetComponent<SimpleInventory>();

            // เช็คว่ามีกุญแจไหม
            if (inventory != null && inventory.HasItem(keyName))
            {
                Debug.Log("ไขกุญแจสำเร็จ!");
                isOpen = true;
                // ตั้งเป้าหมายใหม่ (หมุนแกน Y ไป 90 องศา)
                targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation; 
            }
            else
            {
                Debug.Log("เปิดไม่ได้! ไม่มีกุญแจชื่อ: " + keyName);
            }
        }
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