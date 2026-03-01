using System.Collections;
using UnityEngine;

public class RandomScareSound : MonoBehaviour
{
    [Header("ใส่ไฟล์เสียงหลอนๆ ตรงนี้ (ใส่กี่อันก็ได้)")]
    public AudioClip[] spookySounds;

    [Header("เวลาสุ่มต่ำสุด (วินาที) - 3 นาที = 180")]
    public float minTime = 180f;

    [Header("เวลาสุ่มสูงสุด (วินาที) - 4 นาที = 240")]
    public float maxTime = 240f;

    private AudioSource audioSource;

    void Start()
    {
        // แอบสร้างลำโพงล่องหนขึ้นมาเอง ไม่ต้องนั่งใส่เอง
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // 0 = เสียงแบบ 2D ดังกังวานทั่วฉาก (หลอนดี)
        audioSource.volume = 0.7f;     // ความดัง (ปรับได้ตามชอบ)

        // เริ่มสตาร์ทเครื่องนับเวลา!
        StartCoroutine(RandomSoundRoutine());
    }

    IEnumerator RandomSoundRoutine()
    {
        while (true) // วนลูปทำงานไปเรื่อยๆ ตลอดการเล่นเกม
        {
            // 1. สุ่มเวลาที่จะรอ (ระหว่าง 3 ถึง 4 นาที)
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime); // สั่งให้โค้ดหยุดรอ...

            // 2. พอหมดเวลาปุ๊บ ถ้ามีไฟล์เสียงในช่อง ให้สุ่มเลือกมาเล่น 1 อัน!
            if (spookySounds.Length > 0)
            {
                int randomIndex = Random.Range(0, spookySounds.Length);
                audioSource.PlayOneShot(spookySounds[randomIndex]);

                Debug.Log("👻 เล่นเสียงหลอนแล้ว! รอรอบต่อไปอีก: " + waitTime + " วินาที");
            }
        }
    }
}