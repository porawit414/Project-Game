using UnityEngine;

using TMPro;



public class PlayerInteract : MonoBehaviour

{

    [Header("Settings")]

    public float interactionDistance = 4f; // เพิ่มระยะให้ชัวร์ว่าถึง

    public LayerMask interactLayer;



    [Header("UI")]

    public TextMeshProUGUI interactText;



    [Header("Key")]

    public KeyCode interactKey = KeyCode.E;



    private Camera cam;



    void Start()

    {

        cam = Camera.main;

        if (cam == null) Debug.LogError("❌ หา Main Camera ไม่เจอ! ตรวจสอบว่ากล้องมี Tag 'MainCamera' หรือไม่");

    }



    void Update()

    {

        // สร้างเส้นเลเซอร์สีแดงในหน้า Scene (เอาไว้ดูว่าเส้นยิงไปทางไหน)

        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactionDistance, Color.red);



        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit;



        if (Physics.Raycast(ray, out hit, interactionDistance, interactLayer))

        {

            // ปริ้นท์บอกว่ามองเห็นอะไรอยู่ (ดูใน Console)

            // Debug.Log("มองเห็น: " + hit.collider.name + " | Tag: " + hit.collider.tag);



            if (hit.collider.CompareTag("Door"))

            {

                Debug.Log("✅ เจอประตูแล้ว! กด E ได้เลย"); // ถ้าขึ้นอันนี้แสดงว่า Tag ถูก



                if (interactText != null)

                {

                    interactText.gameObject.SetActive(true);

                    interactText.text = "Press E to open";

                }



                if (Input.GetKeyDown(interactKey))

                {

                    Debug.Log("👉 กด E แล้ว กำลังส่งคำสั่ง ToggleDoor");

                    hit.collider.SendMessage("ToggleDoor", SendMessageOptions.DontRequireReceiver);

                }

            }

            else

            {

                // ถ้ามอง object อื่น ให้ปิด UI

                if (interactText != null) interactText.gameObject.SetActive(false);

            }

        }

        else

        {

            // ถ้ามองอากาศ

            if (interactText != null) interactText.gameObject.SetActive(false);

        }

    }

}