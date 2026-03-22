using UnityEngine;

public class GhostRushSystem : MonoBehaviour
{
    [Header("--- การตั้งค่าการเคลื่อนที่ ---")]
    public float moveSpeed = 10f;       // ความเร็วตอนพุ่งใส่
    public float destroyDistance = 1.2f; // ระยะห่างที่ชนตัวแล้วจะหายไป
    public float lifeTime = 5f;         // วิ่งไม่ถึงใน 5 วิ ให้หายไปเอง

    [Header("--- ระบบเสียง (แยกอิสระ) ---")]
    public AudioClip screamSound;       // ลากไฟล์เสียงร้องมาใส่ใน Inspector
    
    private Transform player;
    private Animator anim;
    private AudioSource myAudioSource;  // ลำโพงส่วนตัวของผีตัวนี้
    private bool isRushing = false;

    void Awake()
    {
        // 1. ค้นหาผู้เล่นผ่าน Tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) 
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("❌ หาวัตถุที่มี Tag 'Player' ไม่เจอ! อย่าลืมตั้ง Tag ที่ตัวละครเรานะครับ");
        }

        anim = GetComponent<Animator>();

        // 2. สร้างลำโพงส่วนตัวให้ผีตัวนี้ (ป้องกันเสียงไปทับตัวอื่น)
        myAudioSource = gameObject.AddComponent<AudioSource>();
        myAudioSource.playOnAwake = false;
        myAudioSource.spatialBlend = 1f; // เป็นเสียง 3D
        myAudioSource.minDistance = 1f;
        myAudioSource.maxDistance = 25f;
    }

    // ฟังก์ชันนี้จะถูกเรียกจากจุดดัก (Trigger)
    public void StartRunning()
    {
        if (isRushing) return;
        isRushing = true;

        // เปลี่ยนท่าเป็นวิ่ง (ชื่อ StartRun ต้องตรงกับใน Animator)
        if (anim != null) anim.SetTrigger("StartRun");

        // เล่นเสียงร้องผ่านลำโพงตัวเองเท่านั้น
        if (screamSound != null) 
        {
            myAudioSource.PlayOneShot(screamSound);
        }

        Debug.Log("👻 ผีเริ่มวิ่งแล้ว!");
    }

    void Update()
    {
        // ถ้ายังไม่ถึงคิววิ่ง หรือหาผู้เล่นไม่เจอ ให้หยุด
        if (!isRushing || player == null) return;

        // 1. หันหน้าหาผู้เล่นตลอดเวลา (ล็อคแกน Y ไม่ให้ผีหน้าทิ่มหรือแหงน)
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPos);

        // 2. เคลื่อนที่ไปหาพิกัดผู้เล่นจริงๆ (MoveTowards ป้องกันผีวิ่งอยู่กับที่)
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 3. ถ้าถึงตัวผู้เล่น ให้หายไป
        if (Vector3.Distance(transform.position, player.position) < destroyDistance)
        {
            Destroy(gameObject);
        }

        // 4. กันเหนียว: ถ้าวิ่งไม่ถึงสักที 5 วิให้หายไป
        Destroy(gameObject, lifeTime);
    }
}