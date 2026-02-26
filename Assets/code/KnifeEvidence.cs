using UnityEngine;

public class KnifeEvidence : MonoBehaviour
{
    [Header("ตัวมีดในฉาก")]
    public GameObject knife3DModel;

    [Header("ช่องหลักฐานในกระเป๋า")]
    public GameObject evidenceUI;

    [Header("ระบบเสียง")]
    public AudioClip pickupSound;

    private bool canPickup = false;

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.F))
        {
            PickUpKnife();
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

    void PickUpKnife()
    {
        // === จุดที่เพิ่ม: สั่งให้ตัวนับหลักฐานทำงาน (+1) ===
        if (GameManager.instance != null)
        {
            GameManager.instance.AddEvidence();
        }

        // 1. เล่นเสียงหยิบมีด
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // 2. เปิดช่องหลักฐาน
        if (evidenceUI != null) evidenceUI.SetActive(true);

        // 3. ซ่อนมีดในฉาก
        if (knife3DModel != null) knife3DModel.SetActive(false);

        canPickup = false;

        // ปิดกล่องชนกันกดซ้ำ
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log("เก็บหลักฐานมีดแล้ว!");
    }
}