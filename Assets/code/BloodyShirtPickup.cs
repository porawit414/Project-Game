using UnityEngine;

public class BloodyShirtPickup : MonoBehaviour
{
    [Header("ตัวเสื้อในฉาก")]
    public GameObject shirt3DModel;

    [Header("ช่องในกระเป๋า")]
    public GameObject shirtUI;

    private bool canPickup = false;

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            PickUpShirt();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }

    void PickUpShirt()
    {
        // เปิดช่องในกระเป๋า
        shirtUI.SetActive(true);

        // ซ่อนเสื้อในฉาก
        shirt3DModel.SetActive(false);

        canPickup = false;
    }
}