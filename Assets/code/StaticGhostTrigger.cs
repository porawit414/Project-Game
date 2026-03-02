using UnityEngine;
using System.Collections;

public class StaticGhostTrigger : MonoBehaviour 
{
    [Header("Setup Objects")]
    public GameObject ghostObject;    // ลาก Static_Ghost มาใส่
    public GameObject ghostModel;     // ลาก Womann:Body มาใส่
    
    [Header("Audio Settings")]
    public AudioSource audioSource;   
    public AudioClip jumpscareSound;  // ลากไฟล์เสียงกรี๊ดมาใส่

    [Header("Settings")]
    public float ghostDuration = 1.0f; // เวลาที่ผีโผล่ (ปรับเหลือ 1 วินาทีแล้ว)

    private bool isGhostActive = false;
    private bool hasScreamed = false;

    // 1. ถ้าเรียกผ่านสคริปต์เก็บของ
    public void OnItemPickedUp() 
    {
        ActivateGhost();
    }

    // 2. ถ้าใช้วิธีเดินเหยียบกล่อง Trigger (จุดดัก)
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) 
        {
            ActivateGhost();
        }
    }

    // ฟังก์ชันสั่งผีออกมาเตรียมตัว
    private void ActivateGhost()
    {
        if (!isGhostActive) 
        {
            isGhostActive = true;
            if (ghostObject != null) ghostObject.SetActive(true);
            StartCoroutine(CheckIfPlayerSeesGhost());
        }
    }

    IEnumerator CheckIfPlayerSeesGhost()
    {
        // ดึง Renderer จาก Womann:Body ที่ลากมาใส่
        Renderer rd = null;
        if (ghostModel != null) 
        {
            rd = ghostModel.GetComponent<Renderer>();
        }

        while (!hasScreamed)
        {
            // ตรวจสอบว่ากล้องของผู้เล่นหันมาเห็นขอบเขตของตัวผีหรือยัง
            if (rd != null && IsVisibleToCamera(rd)) 
            {
                hasScreamed = true;
                
                // สั่งเล่นเสียงทันทีที่ตาเห็น!
                if (audioSource != null && jumpscareSound != null)
                {
                    audioSource.PlayOneShot(jumpscareSound);
                }
                else if (jumpscareSound != null)
                {
                    // กรณีไม่ได้ใส่ AudioSource ไว้ ให้สร้างลำโพงจำลองเล่นเสียงทันที
                    AudioSource.PlayClipAtPoint(jumpscareSound, transform.position);
                }

                // รอ 1 วินาที แล้วปิดผีทิ้ง
                yield return new WaitForSeconds(ghostDuration);
                if (ghostObject != null) ghostObject.SetActive(false);
                
                Destroy(gameObject, 0.5f); // ทำลายจุดดักทิ้งจะได้ไม่ทำงานซ้ำ
                yield break;
            }
            yield return new WaitForSeconds(0.1f); // เช็คทุกๆ 0.1 วินาที
        }
    }

    // ระบบคำนวณหน้ากล้องว่าหันไปเจอผีหรือยัง (แม่นยำที่สุด)
    private bool IsVisibleToCamera(Renderer renderer)
    {
        if (Camera.main == null) return false;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}