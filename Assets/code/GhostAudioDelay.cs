using UnityEngine;
using System.Collections;

public class GhostAudioDelay : MonoBehaviour
{
    [Header("--- ระบบเสียง ---")]
    public AudioClip screamSound; 
    [Range(0f, 1f)] public float volume = 1f; 

    [Header("--- ระบบเคลื่อนที่ (พุ่งครั้งเดียว) ---")]
    public float moveSpeed = 5f;   // ความเร็วในการคลาน
    public float lifeTime = 5f;    // 5 วินาทีหายไปตามที่ต้องการ

    private AudioSource audioSource;
    private Animator anim;
    private bool isRushing = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        
        // จัดการลำโพง
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D เพื่อให้ได้ยินชัดเจน
        audioSource.volume = volume;
    }

    // ⭐ ฟังก์ชันนี้จะถูกเรียกจากจุดดัก (Trigger)
    public void StartRunning()
    {
        // ป้องกันการทำงานซ้ำ (พุ่งครั้งเดียว)
        if (isRushing) return;
        isRushing = true;

        // 1. สั่งเล่นท่าคลานใน Animator
        if (anim != null) 
        {
            anim.SetTrigger("StartRun");
        }

        // 2. เล่นเสียงกรี๊ด
        if (screamSound != null) 
        {
            audioSource.PlayOneShot(screamSound, volume);
        }

        // 3. ⏱️ สั่งทำลายตัวเองทิ้งหลังจากผ่านไป 5 วินาทีพอดี
        Destroy(gameObject, lifeTime);
        
        Debug.Log("👻 ผีเริ่มคลานไปข้างหน้าแล้ว และจะหายไปใน " + lifeTime + " วินาที");
    }

    void Update()
    {
        // ถ้ายังไม่ถึงเวลา หรือสั่งวิ่งไปแล้ว โค้ดส่วนนี้จะเคลื่อนที่ไปข้างหน้าอย่างเดียว
        if (!isRushing) return;

        // 🚀 เคลื่อนที่ไปในทิศทางที่ตัวผีหันหน้าอยู่ (Local Forward)
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}