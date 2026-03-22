using UnityEngine;

public class ShowInteractText : MonoBehaviour
{
    [Header("🌟 ตัวผีที่จะให้โผล่มา")]
    public GameObject ghostEntity; // ลาก Ghost_Waving มาใส่ช่องนี้

    [Header("ตั้งค่า UI")]
    public GameObject uiText; 

    [Header("ชื่อเซฟของมีดเล่มนี้")]
    public string knifeSaveKey = "Evidence_Knife";

    [Header("ตัวมีดในฉากและหน้าต่างหลักฐาน")]
    public GameObject knife3DModel;
    public GameObject evidenceUI;

    [Header("ระบบเสียง")]
    public AudioClip pickupSound;

    private bool isPlayerNear = false;
    private bool hasBeenPickedUp = false; 

    void Start()
    {
        // ถ้าเคยเก็บมีดไปแล้ว ให้ซ่อนผีและปิดโมเดลมีดถาวร
        if (PlayerPrefs.GetInt(knifeSaveKey, 0) == 1)
        {
            if (ghostEntity != null) ghostEntity.SetActive(false); 
            if (evidenceUI != null) evidenceUI.SetActive(true);
            if (knife3DModel != null) knife3DModel.SetActive(false);
            this.enabled = false;
        }
        
        if (uiText != null) uiText.SetActive(false);
        
        // เริ่มเกมมาต้องซ่อนผีไว้ก่อนเสมอ
        if (ghostEntity != null && PlayerPrefs.GetInt(knifeSaveKey, 0) == 0) 
            ghostEntity.SetActive(false);
    }

    void Update()
    {
        // เช็คการกดปุ่ม F เพื่อเก็บของ
        if (isPlayerNear && !hasBeenPickedUp && Input.GetKeyDown(KeyCode.F))
        {
            PickUpKnife();
        }
    }

    void PickUpKnife()
    {
        hasBeenPickedUp = true;

        // เล่นเสียงเก็บของ
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        
        // จัดการ UI และโมเดลมีด
        if (evidenceUI != null) evidenceUI.SetActive(true);
        if (knife3DModel != null) knife3DModel.SetActive(false);
        if (uiText != null) uiText.SetActive(false);

        // 🌟 หัวใจสำคัญ: สั่งให้ผีโผล่มาทันทีที่เก็บมีด!
        if (ghostEntity != null) 
        {
            ghostEntity.SetActive(true);
            Debug.Log("เก็บมีดแล้ว! ผีโผล่มาหลอกทันที");
        }

        // บันทึกสถานะว่าเก็บแล้ว
        PlayerPrefs.SetInt(knifeSaveKey, 1);
        PlayerPrefs.Save();

        this.enabled = false; // ปิดสคริปต์ตัวเอง
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerNear = true;
            if (uiText != null) uiText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (uiText != null) uiText.SetActive(false);
        }
    }
}