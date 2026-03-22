using UnityEngine;
using System.Collections;

public class GhostActivator : MonoBehaviour
{
    [Header("🌟 --- ระบบเซฟความจำ --- 🌟")]
    public string ghostSaveID = "Crawling_Ghost_2"; // เปลี่ยน ID ให้ไม่ซ้ำกับตัวอื่น

    [Header("👻 --- ตั้งค่าตัวผี --- 👻")]
    public GameObject targetGhost;
    public float displayDuration = 5f; // ให้ผีอยู่นาน 5 วิ ตามที่คุณต้องการ

    [Header("🔊 --- ระบบเสียง (Delay 0.4s) --- 🔊")]
    public AudioClip jumpscareSound; 
    public float soundDelay = 0.4f; // 👈 ปรับเป็น 0.4 วินาทีแล้ว
    [Range(0f, 1f)] public float volume = 1f;

    private void Start()
    {
        if (PlayerPrefs.GetInt(ghostSaveID, 0) == 1)
        {
            if (targetGhost != null) Destroy(targetGhost);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(ghostSaveID, 1);
            PlayerPrefs.Save();

            if (targetGhost != null)
            {
                targetGhost.SetActive(true);
                // ส่งคำสั่งให้ผีเริ่มคลาน (จะไปเรียก StartRunning ใน GhostAudioDelay)
                targetGhost.SendMessage("StartRunning", SendMessageOptions.DontRequireReceiver);
            }

            // ⏳ เริ่มนับถอยหลัง 0.4 วิ แล้วค่อยเล่นเสียง
            StartCoroutine(PlaySoundWithDelay());

            // ปิด Collider ทันทีเพื่อไม่ให้ชนซ้ำ (กันเสียงซ้อน)
            GetComponent<Collider>().enabled = false; 
            Destroy(gameObject, soundDelay + 1f); 
        }
    }

    IEnumerator PlaySoundWithDelay()
    {
        yield return new WaitForSeconds(soundDelay);

        if (jumpscareSound != null)
        {
            // เล่นเสียงที่ตำแหน่งจุดดัก
            AudioSource.PlayClipAtPoint(jumpscareSound, transform.position, volume);
            Debug.Log("🔊 [Trigger] เสียงดังขึ้นหลังจากผ่านไป " + soundDelay + " วินาที");
        }
    }
}