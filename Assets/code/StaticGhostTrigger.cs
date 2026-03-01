using UnityEngine;
using System.Collections;
using StarterAssets; //

public class StaticGhostTrigger : MonoBehaviour 
{
    [Header("Setup Objects")]
    public GameObject ghostObject;    
    public Transform eyeTarget;      
    public Transform playerCapsule;   // ลาก PlayerCapsule มาใส่
    
    [Header("Settings")]
    public float rotationDuration = 0.3f; 
    public float ghostDuration = 1.0f; // ปรับเหลือ 1 วิ

    private bool isRotating = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && !isRotating) 
        {
            StartCoroutine(GhostSequence(other.gameObject));
        }
    }

    IEnumerator GhostSequence(GameObject player)
    {
        isRotating = true;

        var controller = player.GetComponent<FirstPersonController>();
        var charController = player.GetComponent<CharacterController>();
        var inputs = player.GetComponent<StarterAssetsInputs>();

        if (controller != null) controller.enabled = false;
        if (charController != null) charController.enabled = false;
        if (inputs != null) 
        {
            inputs.move = Vector2.zero;
            inputs.look = Vector2.zero;
        }

        if (ghostObject != null) ghostObject.SetActive(true); 

        // สะบัดหน้าไปหาผี
        Vector3 targetDir = eyeTarget.position - player.transform.position;
        targetDir.y = 0;
        player.transform.rotation = Quaternion.LookRotation(targetDir);

        float elapsed = 0f;
        while (elapsed < rotationDuration) 
        {
            elapsed += Time.deltaTime;
            if (Camera.main != null) Camera.main.transform.LookAt(eyeTarget);
            yield return null;
        }

        yield return new WaitForSeconds(ghostDuration);

        if (ghostObject != null) ghostObject.SetActive(false);

        // คืนค่าการควบคุมและสั่ง Reset กล้องไม่ให้ดีดขึ้นฟ้า
        if (controller != null)
        {
            controller.enabled = true;
            if (charController != null) charController.enabled = true;

            // เรียกใช้ฟังก์ชันที่เราเพิ่มใน FirstPersonController
            // ถ้าตรงนี้ขึ้นขีดแดง แสดงว่าใน FirstPersonController ยังไม่ได้เพิ่มฟังก์ชันครับ
            player.SendMessage("ForceResetCamera", 0f, SendMessageOptions.DontRequireReceiver);
        }

        isRotating = false;
        Destroy(gameObject); 
    }
}