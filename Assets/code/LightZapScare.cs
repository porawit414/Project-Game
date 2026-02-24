using UnityEngine;

public class LightZapScare : MonoBehaviour
{
    [Header("เสียบสายไฟและเสียง")]
    public Light targetLight;
    public AudioSource audioSource;
    public AudioClip zapSound;
    public AudioClip glassBreakSound;

    [Header("โมเดลหลอดไฟแตก")]
    public GameObject brokenBulbPrefab;

    private Rigidbody rb;
    private bool hasTriggered = false;
    private bool isBroken = false;
    private bool canBreak = false; // 🌟 ตัวล็อคนิรภัย ป้องกันแตกบนเพดาน!

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // 1. ร้องเสียงช็อต + สั่งไฟดับ
            if (zapSound != null) audioSource.PlayOneShot(zapSound);
            if (targetLight != null) targetLight.gameObject.SetActive(false);

            // 2. ถอนตะปู! ปล่อยหลอดไฟร่วงตามแรงโน้มถ่วง
            if (rb != null) rb.isKinematic = false;

            // 🌟 3. สั่งนับถอยหลัง 0.2 วินาที (ให้มันหล่นพ้นเพดานก่อน) ค่อยปลดล็อคให้แตกได้
            Invoke("UnlockBreaking", 0.2f);
        }
    }

    private void UnlockBreaking()
    {
        canBreak = true; // ปลดล็อค! ตอนนี้ถ้าชนอะไรให้แตกได้เลย
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 🌟 ต้องรอให้ canBreak เป็น true ก่อน (พ้น 0.2 วิแรกไปแล้ว) ถึงจะยอมทำงาน
        if (canBreak && !isBroken)
        {
            isBroken = true;

            // 4. เสกเสียงแตกดังเพล้ง! 
            if (glassBreakSound != null)
            {
                AudioSource.PlayClipAtPoint(glassBreakSound, transform.position);
            }

            // 5. เสกเศษหลอดไฟแตก ออกมาแทนที่จุดที่ตกพื้น
            if (brokenBulbPrefab != null)
            {
                Instantiate(brokenBulbPrefab, transform.position, transform.rotation);
            }

            // 6. ทำลายหลอดไฟอันเก่าทิ้ง
            Destroy(gameObject);
        }
    }
}