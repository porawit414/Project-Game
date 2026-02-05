using UnityEngine;

public class SimpleChainCut : MonoBehaviour
{
    [Header("ตั้งค่าระยะและการเก็บของ")]
    public float reachRange = 5.0f; // ระยะเอื้อม (ปรับให้ไกลหน่อยจะได้กดง่าย)
    public bool hasCutter = false;  // เช็คว่าเก็บคีมรึยัง

    [Header("เสียง (ถ้ามี)")]
    public AudioClip cutSound;      // เสียงตัดโซ่

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
                Destroy(hit.transform.gameObject);
                Debug.Log("เก็บคีมแล้ว!");
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
                // เรียกหาไฟล์ประตูตัวใหม่ (FinalChainDoor)
                FinalChainDoor door = hit.transform.GetComponent<FinalChainDoor>();
                if (door != null)
                {
                    door.InteractWithDoor(); // สั่งเปิด/ปิด
                }
            }
        }
    }
}