using UnityEngine;
using System.Collections;

public class MyDoorLock : MonoBehaviour 
{
    [Header("UI แจ้งเตือน (ลากข้อความมาใส่ตรงนี้)")]
    public GameObject lockedMessageUI; 

    [Header("ชื่อกุญแจ")]
    public string keyName = "RoomKey"; 

    [Header("ผี Jumpscare")]
    public GameObject ghostlyWoman; 
    public float ghostDelay = 1f; 

    [Header("การตั้งค่าประตู")]
    public Transform doorHinge;  
    public float openAngle = 90f; 
    public float openSpeed = 2f;  
    public float autoCloseDelay = 5f; 

    private bool isOpen = false; 
    private bool isPlayerNear = false; 
    private Quaternion targetRotation; 
    private Quaternion initialRotation; 
    private Coroutine currentCoroutine; 
    private Collider doorCollider; 

    void Start() 
    {
        if(doorHinge != null) 
        {
            initialRotation = doorHinge.localRotation; 
            targetRotation = initialRotation; 
            doorCollider = doorHinge.GetComponent<Collider>(); 
        }
        
        if(lockedMessageUI != null) lockedMessageUI.SetActive(false);
        if(ghostlyWoman != null) ghostlyWoman.SetActive(false); 
    }

    void Update() 
    {
        // ใช้ปุ่ม E ปุ่มเดียวทั้งเปิดและปิด
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E)) 
        {
            if (!isOpen) 
            {
                TryOpenDoor(); // ถ้าปิดอยู่ ให้พยายามเปิด
            }
            else 
            {
                CloseDoor(); // ถ้าเปิดอยู่ ให้ปิดทันที
            }
        }

        if (doorHinge != null) 
        {
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);

            float angleDifference = Quaternion.Angle(doorHinge.localRotation, initialRotation);
            
            if (angleDifference < 0.5f && !isOpen) 
            {
                if(doorCollider != null) doorCollider.isTrigger = false; 
            }
            else 
            {
                if(doorCollider != null) doorCollider.isTrigger = true; 
            }
        }
    }

    void TryOpenDoor() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); 
        if (player != null) 
        {
            // เช็คกุญแจจาก SimpleInventory ของคุณจอน
            var inventory = player.GetComponent<SimpleInventory>(); 
            if (inventory != null && inventory.HasItem(keyName)) 
            {
                OpenDoor(); 
            }
            else 
            {
                if (currentCoroutine != null) StopCoroutine(currentCoroutine); 
                currentCoroutine = StartCoroutine(ShowLockedMessage()); 
            }
        }
    }

    public void OpenDoor() 
    {
        isOpen = true; 
        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation; 
        
        if (ghostlyWoman != null) StartCoroutine(ShowGhostDelayed()); 

        if (currentCoroutine != null) StopCoroutine(currentCoroutine); 
        currentCoroutine = StartCoroutine(AutoCloseRoutine()); 
    }

    public void CloseDoor() 
    {
        isOpen = false; 
        targetRotation = initialRotation; 
        if (currentCoroutine != null) StopCoroutine(currentCoroutine); 
    }

    IEnumerator ShowLockedMessage() 
    {
        if (lockedMessageUI != null) 
        {
            lockedMessageUI.SetActive(true); 
            yield return new WaitForSeconds(2f); 
            lockedMessageUI.SetActive(false); 
        }
    }

    IEnumerator ShowGhostDelayed() 
    {
        yield return new WaitForSeconds(ghostDelay); 
        if (ghostlyWoman != null) ghostlyWoman.SetActive(true); 
    }

    IEnumerator AutoCloseRoutine() 
    {
        yield return new WaitForSeconds(autoCloseDelay); 
        if (isOpen) CloseDoor(); 
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) isPlayerNear = true; 
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player")) isPlayerNear = false; 
    }
}