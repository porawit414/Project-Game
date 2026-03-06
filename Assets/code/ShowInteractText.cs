using UnityEngine;

public class ShowInteractText : MonoBehaviour
{
    [Header("ตั้งค่า UI")]
    public GameObject uiText; // ลาก InteractText ที่สร้างไว้มาใส่ช่องนี้

    private bool isPlayerNear = false; // ตัวแปรเช็คว่าผู้เล่นอยู่ใกล้ไหม

    void Start()
    {
        // ตอนเริ่มเกม ให้มั่นใจว่าข้อความถูกซ่อนอยู่
        if (uiText != null) uiText.SetActive(false);
    }

    void Update()
    {
        // ถ้าผู้เล่นอยู่ใกล้ และพยายามกดปุ่ม F
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            // คุณจอนสามารถเพิ่มเสียงพึมพำ หรือคำพูดตัวละครตรงนี้ได้
            Debug.Log("ผู้เล่นพยายามเก็บ แต่กระดาษใบนี้ดึงไม่ออก!");
        }
    }

    // ฟังก์ชันทำงานเมื่อผู้เล่นเดินเข้าเขต Collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerNear = true;
            if (uiText != null) uiText.SetActive(true); // โชว์ข้อความ "กด F เพื่อเก็บ"
        }
    }

    // ฟังก์ชันทำงานเมื่อผู้เล่นเดินออกจากเขต Collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (uiText != null) uiText.SetActive(false); // ซ่อนข้อความเมื่อเดินหนี
        }
    }
}