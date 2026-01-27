using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    
    [Header("ลาก KeyMessageCanvas มาใส่ช่องนี้")]
    public GameObject pickupMessage; // <--- ตัวนี้แหละที่นายยังไม่มี!

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && pickupMessage != null)
        {
            pickupMessage.SetActive(true); // เดินเข้า -> โชว์ป้าย
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && pickupMessage != null)
        {
            pickupMessage.SetActive(false); // เดินออก -> ซ่อนป้าย
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SimpleInventory inventory = other.GetComponent<SimpleInventory>();
                ItemPickup itemData = GetComponent<ItemPickup>();

                if (inventory != null && itemData != null)
                {
                    inventory.AddItem(itemData);
                    if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                    // ซ่อนป้ายก่อน
                    if (pickupMessage != null) pickupMessage.SetActive(false);

                    Destroy(gameObject);
                }
            }
        }
    }
}