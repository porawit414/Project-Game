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

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // ถ้าผู้เล่นเดินมาชนจุด Trigger
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // 1. ร้องเสียงไฟช็อต + สั่งไฟดับ
            if (zapSound != null) audioSource.PlayOneShot(zapSound);
            if (targetLight != null) targetLight.gameObject.SetActive(false);

            // 2. เสกเสียงแตกดังเพล้ง! ออกมาจากจุดที่เพดาน
            if (glassBreakSound != null)
            {
                AudioSource.PlayClipAtPoint(glassBreakSound, transform.position);
            }

            // 3. เสกเศษหลอดไฟแตก ออกมาแทนที่จุดเดิมบนเพดาน 
            // (ถ้าเศษกระจกใน Prefab มี Rigidbody มันจะร่วงกราวลงพื้นเองอย่างสวยงามครับ!)
            if (brokenBulbPrefab != null)
            {
                Instantiate(brokenBulbPrefab, transform.position, transform.rotation);
            }

            // 4. ทำลายโมเดลหลอดไฟดวงเก่า(ที่ยังไม่แตก)ทิ้งไปซะ!
            Destroy(gameObject);
        }
    }
}