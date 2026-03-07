using UnityEngine;

public class AutoDoorSound : MonoBehaviour
{
    [Header("ใส่เสียงเปิด-ปิดประตู")]
    public AudioClip openSound;
    public AudioClip closeSound;

    private AudioSource audioSource;
    private Quaternion closedRotation;
    private bool isDoorOpen = false;

    void Start()
    {
        // สร้างระบบเสียง 3 มิติให้อัตโนมัติ (เสียงจะดังมาจากที่ตั้งของประตู)
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 1 = เสียง 3 มิติ (เดินออกห่างเสียงจะเบาลง)

        // จำองศาตอนที่ประตูปิดสนิทไว้ตั้งแต่เริ่มเกม
        closedRotation = transform.localRotation;
    }

    void Update()
    {
        // แอบวัดองศาว่าประตูขยับไปจากจุดเดิมกี่องศาแล้ว?
        float angleDifference = Quaternion.Angle(transform.localRotation, closedRotation);

        // ถ้าประตูขยับเกิน 5 องศา (แปลว่ากำลังเปิด) และก่อนหน้านี้มันปิดอยู่
        if (angleDifference > 5f && !isDoorOpen)
        {
            isDoorOpen = true; // จำไว้ว่าเปิดแล้ว
            if (openSound != null) audioSource.PlayOneShot(openSound);
        }
        // ถ้าประตูกลับมาที่เดิม (องศาน้อยกว่า 5) และก่อนหน้านี้มันเปิดอยู่
        else if (angleDifference < 5f && isDoorOpen)
        {
            isDoorOpen = false; // จำไว้ว่าปิดแล้ว
            if (closeSound != null) audioSource.PlayOneShot(closeSound);
        }
    }
}