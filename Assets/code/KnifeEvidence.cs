using UnityEngine;

public class KnifeEvidence : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของมีดเล่มนี้ (ห้ามซ้ำ)")]
    public string knifeSaveKey = "Evidence_Knife";

    [Header("ตัวมีดในฉาก")]
    public GameObject knife3DModel;

    [Header("ช่องหลักฐานในกระเป๋า")]
    public GameObject evidenceUI;

    [Header("ระบบเสียง")]
    public AudioClip pickupSound;

    private bool canPickup = false;

    void Start()
    {
        // 🌟 1. เช็คตอนเริ่มเกมว่า "เคยเก็บมีดเล่มนี้ไปหรือยัง?"
        // ถ้า PlayerPrefs มีค่าเป็น 1 แปลว่าเคยเก็บแล้ว
        if (PlayerPrefs.GetInt(knifeSaveKey, 0) == 1)
        {
            // เปิดช่องหลักฐานในกระเป๋ารอไว้เลย
            if (evidenceUI != null) evidenceUI.SetActive(true);

            // ซ่อนมีดในฉาก
            if (knife3DModel != null) knife3DModel.SetActive(false);

            // ปิดกล่องชนและสคริปต์นี้ทิ้งไปเลย จะได้ไม่ต้องเดินมาเก็บซ้ำ
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
            this.enabled = false;
        }
    }

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

        // 🌟 2. เซฟลงเครื่องว่า "เก็บมีดเปื้อนเลือดไปแล้ว! (ค่า = 1)"
        PlayerPrefs.SetInt(knifeSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บหลักฐานมีดแล้ว!");

        // ปิดการทำงานสคริปต์กันเหนียว
        this.enabled = false;
    }
}