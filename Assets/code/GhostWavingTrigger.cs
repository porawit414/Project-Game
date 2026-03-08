using UnityEngine;

public class GhostWavingTrigger : MonoBehaviour
{
    [Header("ลาก Ghost_Waving มาใส่ที่นี่")]
    public GameObject ghostObject; 
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // ถ้าชนผู้เล่นปุ๊บ ผีโผล่ปั๊บ (เพราะสคริปต์นี้จะถูกเปิดใช้งานเมื่อมีดหายไปเท่านั้น)
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (ghostObject != null) 
            {
                ghostObject.SetActive(true);
                hasTriggered = true;
                Debug.Log("Success! ผู้เล่นเดินชนจุดดัก ผีออกมาโบกมือแล้ว");
            }
        }
    }
}