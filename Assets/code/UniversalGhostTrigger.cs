using UnityEngine;

public class UniversalGhostTrigger : MonoBehaviour
{
    public GameObject ghostObject; 
    public GhostRushSystem rushScript; 
    public bool isSpawnTrigger = true; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isSpawnTrigger)
            {
                // จุดที่ 1: มั่นใจว่าสั่งแค่โผล่ ไม่สั่งวิ่ง
                if (ghostObject != null) 
                {
                    ghostObject.SetActive(true);
                    Debug.Log("Ghost Spawned! Standing still...");
                }
            }
            else
            {
                // จุดที่ 2: สั่งวิ่ง
                if (rushScript != null) 
                {
                    rushScript.StartRunning();
                    Debug.Log("Ghost is Rushing now!");
                }
            }

            Destroy(gameObject); 
        }
    }
}