using UnityEngine;

public class KnifeEvidence : MonoBehaviour
{
    [Header("🌟 ลากวัตถุ Ghost_Spawn_Trigger มาใส่ที่นี่")]
    public GameObject ghostSpawnTrigger; 

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
        // เช็คตอนเริ่มเกมว่าเคยเก็บไปหรือยัง
        if (PlayerPrefs.GetInt(knifeSaveKey, 0) == 1)
        {
            if (evidenceUI != null) evidenceUI.SetActive(true);
            if (knife3DModel != null) knife3DModel.SetActive(false);

            // ถ้าเคยเก็บแล้ว ก็สั่งเปิดจุดดักผีรอไว้เลย
            if (ghostSpawnTrigger != null) ghostSpawnTrigger.SetActive(true);

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
        if (other.CompareTag("Player")) canPickup = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) canPickup = false;
    }

    void PickUpKnife()
    {
        if (GameManager.instance != null) GameManager.instance.AddEvidence();

        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        if (evidenceUI != null) evidenceUI.SetActive(true);
        if (knife3DModel != null) knife3DModel.SetActive(false); // มีดหายไปจากฉาก

        // 🌟 สั่งให้จุดดักผีทำงานทันทีที่มีดหายไป!
        if (ghostSpawnTrigger != null) 
        {
            ghostSpawnTrigger.SetActive(true);
            Debug.Log("มีดหายไปแล้ว -> เปิดใช้งานจุดดักผีทันที!");
        }

        canPickup = false;
        PlayerPrefs.SetInt(knifeSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บหลักฐานมีดแล้ว!");
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        this.enabled = false;
    }
}