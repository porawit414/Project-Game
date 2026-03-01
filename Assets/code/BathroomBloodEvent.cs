using System.Collections;
using UnityEngine;

public class BathroomBloodEvent : MonoBehaviour
{
    [Header("เวลาที่ต้องรอก่อนเลือดโผล่ (วินาที)")]
    public float timeToWait = 180f; // 3 นาที = 180 วินาที

    [Header("คราบเลือดที่กำแพงห้องน้ำ (ลากมาใส่)")]
    public GameObject bloodStain;

    [Header("เสียงหลอนตอนเลือดโผล่ (เช่น เสียงกระจกแตก/เสียงกรี๊ด)")]
    public AudioClip spookySound;

    private AudioSource audioSource;

    void Start()
    {
        // 1. ซ่อนรอยเลือดไว้ก่อนทันทีที่เริ่มเกม!
        if (bloodStain != null)
        {
            bloodStain.SetActive(false);
        }

        // 2. สร้างลำโพงล่องหนสำหรับเล่นเสียงหลอน
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // 0 = เสียง 2D ดังกังวานทั่วบ้าน ให้คนเล่นได้ยินชัวร์ๆ
        audioSource.volume = 0.8f;

        // 3. เริ่มนับเวลาถอยหลัง!
        StartCoroutine(TriggerBloodEvent());
    }

    IEnumerator TriggerBloodEvent()
    {
        // สั่งให้ระบบหยุดรอเป็นเวลา 3 นาที (180 วินาที)
        yield return new WaitForSeconds(timeToWait);

        // --- พอครบ 3 นาทีปุ๊บ คำสั่งด้านล่างนี้จะทำงานทันที! ---

        // 1. โชว์รอยเลือดบนกำแพง!
        if (bloodStain != null)
        {
            bloodStain.SetActive(true);
        }

        // 2. เล่นเสียงหลอนดึงดูดความสนใจ!
        if (spookySound != null)
        {
            audioSource.PlayOneShot(spookySound);
        }

        Debug.Log("👻 3 นาทีแล้ว! รอยเลือดในห้องน้ำโผล่มาแล้ว!");
    }
}