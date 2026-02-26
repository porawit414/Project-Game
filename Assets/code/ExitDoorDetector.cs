using UnityEngine;

public class ExitDoorDetector : MonoBehaviour 
{
    [Header("เชื่อมต่อกับประตู")]
    public SimpleDoorController doorController; 

    private void OnTriggerEnter(Collider other) 
    {
        // ตรวจสอบว่าสิ่งที่เดินมาชนคือ Player และประตูเปิดอยู่หรือไม่
        if (other.CompareTag("Player") && doorController != null) 
        {
            // สั่งให้สคริปต์ประตูเริ่มทำให้จอมืดลง
            doorController.StartEndingSequence(); 
        }
    }
}