using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public Animator doorAnimator; // ลาก Animator ของประตูมาใส่ในช่องนี้ (ถ้าใช้ Animation)
    private bool isOpen = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ถ้ากด E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // เช็คว่าผู้เล่นมีกุญแจไหม
                PlayerInventory playerInv = other.GetComponent<PlayerInventory>();

                if (playerInv != null && playerInv.hasKey == true)
                {
                    OpenTheDoor();
                }
                else
                {
                    Debug.Log("ประตูล็อค! ต้องหากุญแจก่อน");
                }
            }
        }
    }

    void OpenTheDoor()
    {
        if (!isOpen)
        {
            // ตรงนี้ใส่โค้ดเปิดประตูที่คุณทำไว้แล้วได้เลย
            // เช่น:
            // doorAnimator.SetTrigger("Open"); 
            // หรือหมุนประตู: transform.Rotate(0, 90, 0);
            
            Debug.Log("เปิดประตูสำเร็จ!");
            isOpen = true;
        }
    }
}