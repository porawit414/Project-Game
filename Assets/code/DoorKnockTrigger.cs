using UnityEngine;
using System.Collections;

public class DoorKnockTrigger : MonoBehaviour
{
    [Header("อ้างอิงไปที่ประตู (เพื่อเช็คว่าปลดล็อคหรือยัง)")]
    public FinalChainDoor targetDoor; 

    [Header("ตั้งค่าเสียงเคาะ")]
    public AudioClip knockSound;
    public float knockDelay = 2.5f;

    private AudioSource audioSource;
    private Coroutine knockRoutine;
    private bool isPlayerInside = false;

    void Start()
    {
        // สร้างเครื่องเล่นเสียงให้กล่องดักอัตโนมัติ
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // ให้เป็นเสียง 3 มิติ (ดังมาจากจุดที่วางกล่องไว้)
    }

    private void OnTriggerEnter(Collider other)
    {
        // ถ้าผู้เล่นเดินเข้ามาในกล่องดัก
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            // เช็คโค้ดประตูของคุณ: ถ้าโซ่ (chainLock) หายไปแล้ว แปลว่าประตูเปิดได้แล้ว!
            // ถ้าเปิดได้แล้ว ให้ทำลายกล่องดักนี้ทิ้งไปเลย จะได้ไม่มีเสียงเคาะอีกถาวร
            if (targetDoor != null && targetDoor.chainLock == null)
            {
                Destroy(gameObject);
                return;
            }

            // ถ้าประตูด้านหน้ายังติดโซ่อยู่ ก็เริ่มเคาะขู่เลย!
            if (knockRoutine == null)
            {
                knockRoutine = StartCoroutine(AutoKnock());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ถ้าผู้เล่นเดินหนีออกจากกล่องดัก ให้หยุดเคาะ
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (knockRoutine != null)
            {
                StopCoroutine(knockRoutine);
                knockRoutine = null;
            }
        }
    }

    IEnumerator AutoKnock()
    {
        while (isPlayerInside)
        {
            // เช็คตลอดเวลาที่เคาะ: ถ้าจู่ๆ ผู้เล่นตัดโซ่ได้ตอนที่กำลังเคาะอยู่
            if (targetDoor != null && targetDoor.chainLock == null)
            {
                Destroy(gameObject); // ทำลายกล่องเสียงทิ้งทันที
                yield break; // สั่งหยุดการทำงานของลูป
            }

            // เล่นเสียงเคาะ
            if (knockSound != null)
            {
                audioSource.PlayOneShot(knockSound);
            }
            
            // รอเวลาตามที่ตั้งไว้ แล้ววนกลับไปเคาะใหม่
            yield return new WaitForSeconds(knockDelay);
        }
    }
}