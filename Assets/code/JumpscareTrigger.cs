using UnityEngine;
using System.Collections;

public class JumpscareTrigger : MonoBehaviour 
{
    public GameObject ghostUI;    // ลากรูปผีมาใส่
    public AudioSource scareSound; // ลาก AudioSource ที่มีเสียงกรี๊ดมาใส่

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) 
        {
            StartCoroutine(ShowGhostTime());
        }
    }

    IEnumerator ShowGhostTime() 
    {
        ghostUI.SetActive(true);      // 1. โชว์รูปผี
        if (scareSound != null) scareSound.Play(); // 2. เล่นเสียงกรี๊ด
        
        yield return new WaitForSeconds(0.5f); // 3. รอ 0.5 วินาที

        ghostUI.SetActive(false);     // 4. ซ่อนรูปผี
        Destroy(gameObject);          // 5. ทำลายจุดดักทิ้งตลอดกาล
    }
}