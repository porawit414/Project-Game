using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f; // องศาที่จะเปิด
    public float openSpeed = 2f;  // ความเร็วในการเปิด
    
    private bool isOpen = false;
    private Quaternion defaultRotation;
    private Quaternion targetRotation;

    void Start()
    {
        defaultRotation = transform.rotation;
        // คำนวณมุมที่จะเปิด (หมุนแกน Y)
        targetRotation = Quaternion.Euler(0, openAngle, 0) * defaultRotation;
    }

    void Update()
    {
        // ถ้าสถานะคือเปิด ให้ค่อยๆ หมุนประตูไปยังมุมเป้าหมาย
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            
            // ถ้ามี Inventory และ มีกุญแจ (hasKey เป็น true)
            if (inventory != null && inventory.hasKey == true)
            {
                isOpen = true; // สั่งให้เปิดประตู
                Debug.Log("ประตูเปิดแล้ว!");
            }
            else
            {
                Debug.Log("ประตูล็อค! ต้องไปหากุญแจมาก่อน");
            }
        }
    }
}