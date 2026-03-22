using UnityEngine;

public class GhostRushSystem : MonoBehaviour
{
    [Header("--- การตั้งค่าการเคลื่อนที่ ---")]
    public float moveSpeed = 10f;       
    public float destroyDistance = 1.2f; 
    public float lifeTime = 5f;         

    [Header("--- ระบบเสียง (เน้นดังสะใจ) ---")]
    public AudioClip screamSound;       
    [Range(0f, 1f)] public float volume = 1f; // ตั้งค่าความดังได้จาก Inspector (ค่าเริ่มต้นคือ 1 = ดังสุด)
    
    private Transform player;
    private Animator anim;
    private AudioSource myAudioSource;  
    private bool isRushing = false;

    void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        anim = GetComponent<Animator>();

        // --- ตั้งค่าลำโพงให้ดังที่สุด ---
        myAudioSource = gameObject.AddComponent<AudioSource>();
        myAudioSource.playOnAwake = false;
        
        // ปรับเป็น 0.2f เพื่อให้ยังพอรู้ทิศทางผีบ้าง แต่เสียงจะดังชัดเหมือนอยู่ข้างหูตลอด
        myAudioSource.spatialBlend = 0.2f; 
        
        myAudioSource.volume = volume;         // เร่ง Volume เต็มที่
        myAudioSource.priority = 0;            // ตั้งค่า Priority เป็น 0 (สำคัญที่สุด) เพื่อไม่ให้เสียงอื่นมากลบ
        myAudioSource.minDistance = 10f;       // ระยะ 10 เมตรแรกจะดัง 100% ตลอด
        myAudioSource.maxDistance = 100f;      // ได้ยินเสียงไกลถึง 100 เมตร
        myAudioSource.rolloffMode = AudioRolloffMode.Linear; // ให้เสียงค่อยๆ เบาลงแบบเส้นตรง (จะดังกว่าแบบปกติ)
    }

    public void StartRunning()
    {
        if (isRushing) return;
        isRushing = true;

        if (anim != null) anim.SetTrigger("StartRun");

        if (screamSound != null) 
        {
            // ใช้ PlayOneShot ร่วมกับ Volume ที่ตั้งไว้
            myAudioSource.PlayOneShot(screamSound, volume);
        }

        Destroy(gameObject, lifeTime);
        Debug.Log("👻 ผีเริ่มวิ่งแล้ว! (เสียงดังสุดๆ)");
    }

    void Update()
    {
        if (!isRushing || player == null) return;

        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPos);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) < destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}