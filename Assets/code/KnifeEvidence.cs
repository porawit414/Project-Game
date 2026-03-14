using UnityEngine;

public class KnifeGhostEvidence : MonoBehaviour
{
    [Header("🌟 ลากวัตถุจุดเสกผี และ จุดทำให้ผีหาย มาใส่ที่นี่")]
    public GameObject ghostSpawnTrigger; 
    public GameObject ghostHideTrigger; // 🌟 ช่องใหม่สำหรับลาก Trigger_Ghost_Waving มาใส่!

    [Header("🌟 ชื่อเซฟของมีดเล่มนี้ (ห้ามซ้ำ)")]
    public string knifeSaveKey = "Evidence_Knife";

    [Header("ตัวมีดในฉาก")]
    public GameObject knife3DModel;

    [Header("ช่องหลักฐานในกระเป๋า")]
    public GameObject evidenceUI;

    [Header("ระบบเสียง")]
    public AudioClip pickupSound;

    private bool canPickup = false;
    private bool hasBeenPickedUp = false; // 🌟 ตัวล็อคกันกด F เบิ้ล

    void Start()
    {
        // เช็คตอนเริ่มเกมว่าเคยเก็บไปหรือยัง
        if (PlayerPrefs.GetInt(knifeSaveKey, 0) == 1)
        {
            if (evidenceUI != null) evidenceUI.SetActive(true);
            if (knife3DModel != null) knife3DModel.SetActive(false);

            // ถ้าเคยเก็บแล้ว ก็สั่งเปิดจุดดักผีทั้ง 2 จุดรอไว้เลย
            if (ghostSpawnTrigger != null) ghostSpawnTrigger.SetActive(true);
            if (ghostHideTrigger != null) ghostHideTrigger.SetActive(true);

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
            this.enabled = false;
        }
    }

    void Update()
    {
        // เช็คเงื่อนไขและต้องยังไม่เคยถูกเก็บมาก่อน
        if (canPickup && !hasBeenPickedUp && Input.GetKeyDown(KeyCode.F))
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
        hasBeenPickedUp = true; // ล็อคทันทีกันบั๊กบวกเลขเบิ้ล

        if (GameManager.instance != null) GameManager.instance.AddEvidence();

        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        if (evidenceUI != null) evidenceUI.SetActive(true);
        if (knife3DModel != null) knife3DModel.SetActive(false); // มีดหายไปจากฉาก

        // 🌟 สั่งให้จุดดักผีทำงานทันทีที่มีดหายไป! (เปิดทั้งจุดโผล่และจุดหาย)
        if (ghostSpawnTrigger != null) ghostSpawnTrigger.SetActive(true);
        if (ghostHideTrigger != null) ghostHideTrigger.SetActive(true);
        
        Debug.Log("มีดหายไปแล้ว -> เปิดใช้งานจุดดักผีทั้ง 2 จุดทันที!");

        canPickup = false;
        PlayerPrefs.SetInt(knifeSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บหลักฐานมีดแล้ว! ผีพร้อมทำงาน!");
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        this.enabled = false;
    }
}