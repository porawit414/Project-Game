using UnityEngine;
using System.Collections;

public class GhostAudioDelay : MonoBehaviour
{
    [Header("--- ระบบเสียง ---")]
    public AudioClip screamSound; // ลากไฟล์เสียงกรี๊ดมาใส่
    public float soundDelay = 0.2f; 
    
    [Header("--- ระบบเคลื่อนที่ ---")]
    public float moveSpeed = 8f;
    public float destroyDistance = 1.2f;

    private AudioSource audioSource;
    private Animator anim;
    private Transform player;
    private bool isRushing = false; // ตัวเช็คว่าถึงเวลาวิ่งหรือยัง

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // ตั้งค่าเสียง 3D เหมือนเดิม
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; 
    }

    // ⭐ ฟังก์ชันใหม่: สั่งให้ผีเริ่มวิ่ง (เรียกจากจุดดัก Trigger)
    public void StartRunning()
    {
        if (isRushing) return;
        isRushing = true;

        // เปลี่ยนท่าเป็นวิ่งใน Animator
        if (anim != null) anim.SetTrigger("StartRun");

        // เล่นเสียงกรี๊ด
        if (screamSound != null) audioSource.PlayOneShot(screamSound);
    }

    void Update()
    {
        // ถ้ายังไม่สั่งให้วิ่ง (isRushing เป็น false) ผีจะยืนโบกมือเฉยๆ
        if (!isRushing || player == null) return;

        // โค้ดสั่งวิ่งเข้าหาผู้เล่น
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPos);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // ถึงตัวแล้วหายไป
        if (Vector3.Distance(transform.position, player.position) < destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}