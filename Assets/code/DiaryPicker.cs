using UnityEngine;

public class DiaryPicker : MonoBehaviour
{
    public float interactDistance = 10f;
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerCamera == null) return;

            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            // ใช้ RaycastAll ยิงทะลุทุกอย่าง! ป้องกันเลเซอร์ติดพุงตัวเองหรือโซนล่องหน
            RaycastHit[] hits = Physics.RaycastAll(ray, interactDistance);

            foreach (RaycastHit hit in hits)
            {
                // ถ้าในบรรดาของที่ยิงทะลุไป มีชิ้นไหน Tag ว่า Interactable
                if (hit.collider.CompareTag("Interactable"))
                {
                    DiaryObject diary = hit.collider.GetComponent<DiaryObject>();
                    if (diary != null)
                    {
                        diary.Interact();
                        Debug.Log("✅ เก็บสมุดสำเร็จแล้วโว้ยยย!");
                        return; // เก็บเสร็จให้หยุดหาเลย
                    }
                }
            }
        }
    }
}