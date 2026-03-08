using UnityEngine;

public class EvidenceItem : MonoBehaviour
{
    public string evidenceName = "Bloody Knife";

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // ใส่ // ไว้ข้างหน้าบรรทัดที่พัง เพื่อปิดการทำงานชั่วคราว
                // Inventory.instance.AddEvidence(evidenceName); 
                
                Debug.Log("เก็บหลักฐาน: " + evidenceName + " (บรรทัด Inventory ถูกปิดไว้เพื่อแก้ Error)");
                
                Destroy(gameObject);
            }
        }
    }
}