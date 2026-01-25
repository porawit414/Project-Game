using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip pickupSound; // ลากไฟล์เสียงมาใส่ช่องนี้ใน Inspector

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerInventory playerInv = other.GetComponent<PlayerInventory>();
                
                if (playerInv != null)
                {
                    // 1. ให้กุญแจ
                    playerInv.hasKey = true;

                    // 2. สั่งให้ Player โชว์ข้อความ
                    playerInv.ShowNotification("เก็บกุญแจห้องนอนแล้ว!");

                    // 3. เล่นเสียง (PlayClipAtPoint ดีที่สุดสำหรับของที่กำลังจะถูกลบ)
                    if (pickupSound != null)
                    {
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }

                    // 4. ลบกุญแจ
                    Destroy(gameObject);
                }
            }
        }
    }
}