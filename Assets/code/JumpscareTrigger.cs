using UnityEngine;
using System.Collections;

public class JumpscareTrigger : MonoBehaviour 
{
    public GameObject ghostUI;    // ลากรูปผีมาใส่
    public AudioSource scareSound; // ลาก AudioSource เสียงกรี๊ดมาใส่
    
    private int walkCount = 0;    // ตัวนับว่าเดินผ่านกี่ครั้งแล้ว

    private void OnTriggerEnter(Collider other) 
    {
        // เช็คว่าเป็น Player หรือเปล่า
        if (other.CompareTag("Player")) 
        {
            walkCount++; // บวกเพิ่ม 1 ทุกครั้งที่เดินชน Trigger

            // ถ้าเดินผ่านเป็นครั้งที่ 2 ถึงจะทำงาน
            if (walkCount >= 2) 
            {
                StartCoroutine(ShowGhostTime());
            }
        }
    }

    IEnumerator ShowGhostTime() 
    {
        ghostUI.SetActive(true);      // 1. โชว์รูปผี
        if (scareSound != null) scareSound.Play(); // 2. เล่นเสียงกรี๊ด
        
        yield return new WaitForSeconds(0.5f); // 3. โชว์ค้างไว้ 0.5 วินาที

        ghostUI.SetActive(false);     // 4. ซ่อนรูปผี
        Destroy(gameObject);          // 5. ทำลายจุดดักทิ้ง (หลอกเสร็จแล้วทำลายเลย)
    }
}