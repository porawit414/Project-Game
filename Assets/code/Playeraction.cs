using UnityEngine;

public class SimpleChainCut : MonoBehaviour
{
    [Header("ตั้งค่าระยะและการเก็บของ")]
    public float reachRange = 5.0f; 
    public bool hasCutter = false;  

    [Header("UI ในกระเป๋า (ลากรูป Icon_Cutter มาใส่ช่องนี้)")] 
    public GameObject cutterUIIcon; // <--- ตัวแปรใหม่สำหรับใส่รูปครับ

    [Header("เสียง (ถ้ามี)")]
    public AudioClip cutSound;      

    void Update()
    {
        // 1. กด F เพื่อ "เก็บคีม" หรือ "ตัดโซ่"
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteractF();
        }

        // 2. กด E เพื่อ "เปิด/ปิด ประตู"
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractE();
        }
    }

    // ฟังก์ชันสำหรับปุ่ม F (จัดการคีมและโซ่)
    void TryInteractF()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachRange))
        {
            // --- เจอกรรไกร/คีม ---
            if (hit.transform.CompareTag("Cutter"))
            {
                hasCutter = true;
                
                // >>> สั่งเปิดรูปในกระเป๋าตรงนี้! <<<
                if (cutterUIIcon != null)
                {
                    cutterUIIcon.SetActive(true);
                }

                Destroy(hit.transform.gameObject); // ลบคีมบนพื้นทิ้ง
                Debug.Log("เก็บคีมเข้ากระเป๋าเรียบร้อย!");
            }
            // --- เจอโซ่ ---
            else if (hit.transform.CompareTag("Chain"))
            {
                if (hasCutter)
                {
                    if (cutSound != null) AudioSource.PlayClipAtPoint(cutSound, hit.transform.position);
                    Destroy(hit.transform.gameObject);
                    Debug.Log("ตัดโซ่ขาด!");
                }
                else
                {
                    Debug.Log("ไม่มีคีม! ต้องไปหามาก่อน");
                }
            }
        }
    }

    // ฟังก์ชันสำหรับปุ่ม E (จัดการประตู)
    void TryInteractE()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachRange))
        {
            // --- เจอประตู (ต้อง Tag ว่า Door) ---
            if (hit.transform.CompareTag("Door"))
            {
                FinalChainDoor door = hit.transform.GetComponent<FinalChainDoor>();
                if (door != null)
                {
                    door.InteractWithDoor(); 
                }
            }
        }
    }
}