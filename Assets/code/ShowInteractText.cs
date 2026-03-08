using UnityEngine;

public class ShowInteractText : MonoBehaviour
{
    [Header("🌟 ลากจุดเสกผี (จุดที่ 1) และจุดทำให้ผีหาย (จุดที่ 2) มาใส่")]
    public GameObject ghostSpawnTrigger; 
    public GameObject ghostHideTrigger;

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
    private bool hasBeenPickedUp = false; // กันบั๊กกดรัว

    void Start()
    {
        if (PlayerPrefs.GetInt(knifeSaveKey, 0) == 1)
        {
            ActivateTriggers(); 
            if (evidenceUI != null) evidenceUI.SetActive(true);
            if (knife3DModel != null) knife3DModel.SetActive(false);
            this.enabled = false;
        }
        
        if (uiText != null) uiText.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && !hasBeenPickedUp && Input.GetKeyDown(KeyCode.F))
        {
            PickUpKnife();
        }
    }

    void PickUpKnife()
    {
        hasBeenPickedUp = true;

        // ❌ ลบบรรทัด GameManager.instance.AddEvidence() ออกแล้ว 
        // เพื่อไม่ให้เลขบวกซ้ำซ้อน (ให้สคริปต์หลักเป็นคนบวกแทน)

        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        if (evidenceUI != null) evidenceUI.SetActive(true);
        if (knife3DModel != null) knife3DModel.SetActive(false);
        if (uiText != null) uiText.SetActive(false);

        ActivateTriggers();

        PlayerPrefs.SetInt(knifeSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บมีดแล้ว -> เปิดระบบผี (ไม่มีการบวกเลขจากสคริปต์นี้)");
        this.enabled = false;
    }

    void ActivateTriggers()
    {
        if (ghostSpawnTrigger != null) ghostSpawnTrigger.SetActive(true);
        if (ghostHideTrigger != null) ghostHideTrigger.SetActive(true);
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