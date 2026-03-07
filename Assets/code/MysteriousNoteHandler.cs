using UnityEngine;
using System.Collections;

public class MysteriousNoteHandler : MonoBehaviour
{
    [Header("🌟 ชื่อเซฟของกระดาษโน้ต (ห้ามซ้ำ)")]
    public string noteSaveKey = "Item_NumberNote"; 

    [Header("UI Settings")]
    public GameObject noteInventoryButton; 
    public GameObject noteReadPanel;       
    public GameObject interactMessage;     

    [Header("Ghost System")]
    public GameObject ghostTrigger;        

    [Header("Audio")]
    public AudioClip pickupSound;

    private bool canPickup = false;
    private bool isReading = false;

    void Start()
    {
        if (PlayerPrefs.GetInt(noteSaveKey, 0) == 1)
        {
            if (noteInventoryButton != null) noteInventoryButton.SetActive(true);

            if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;
            if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
            
            if (interactMessage != null) interactMessage.SetActive(false); 

            this.enabled = false;
        }
        else
        {
            if (interactMessage != null) interactMessage.SetActive(false);
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.F) && !isReading)
        {
            PickUpNote();
        }

        if (isReading && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            if (interactMessage != null) interactMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
            if (interactMessage != null) interactMessage.SetActive(false);
        }
    }

    void PickUpNote()
    {
        if (noteInventoryButton != null) noteInventoryButton.SetActive(true);

        if (ghostTrigger != null)
        {
            ghostTrigger.SetActive(true);
            Debug.Log("ระบบผีหลอกเปิดใช้งานแล้ว! เตรียมตัวหันหลังได้เลย...");
        }

        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        if (GetComponent<MeshRenderer>() != null) gameObject.GetComponent<MeshRenderer>().enabled = false;
        if (GetComponent<Collider>() != null) gameObject.GetComponent<Collider>().enabled = false;

        if (interactMessage != null) interactMessage.SetActive(false);

        PlayerPrefs.SetInt(noteSaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("เก็บกระดาษแล้ว!");

        this.enabled = false;
    }

    public void OpenNote()
    {
        if (noteReadPanel != null)
        {
            noteReadPanel.SetActive(true);
            isReading = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseNote()
    {
        if (noteReadPanel != null)
        {
            noteReadPanel.SetActive(false);
            isReading = false;
        }
    }
}