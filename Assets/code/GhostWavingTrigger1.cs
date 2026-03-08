using UnityEngine;

public class Trigger_Ghost_Waving : MonoBehaviour
{
    [Header("ลาก Ghost_Waving มาใส่ที่นี่")]
    public GameObject ghostObject; 

    [Header("ชื่อ Key ที่ใช้เซฟตอนเก็บมีด")]
    public string knifeSaveKey = "Evidence_Knife";

    private bool hasTriggered = false;

    // ไม่ต้องมี void Start() ที่สั่งปิดผี เพราะหน้าที่นั้นเป็นของจุดเสกผีครับ

    private void OnTriggerEnter(Collider other)
    {
        // เช็ค 1: เป็นผู้เล่น
        // เช็ค 2: จุดนี้ยังไม่เคยทำงาน (hasTriggered)
        if (other.CompareTag("Player") && !hasTriggered)
        {
            // เช็ค 3: เก็บมีดแล้วหรือยัง (ค่าความจำต้องเป็น 1)
            if (PlayerPrefs.GetInt(knifeSaveKey, 0) == 1)
            {
                if (ghostObject != null)
                {
                    ghostObject.SetActive(false); // สั่งปิดตัวผีทันที
                    Debug.Log("Jon Kimson เดินมาถึงจุดดักที่ 2: ผีหายไปแล้ว!");
                }

                hasTriggered = true; 
                Destroy(gameObject, 0.1f); // ทำลายจุดดักทิ้งเพื่อประหยัดทรัพยากร
            }
            else
            {
                Debug.Log("ยังไม่ได้เก็บมีด จุดทำให้ผีหายจะไม่ทำงาน");
            }
        }
    }
}