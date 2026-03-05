using UnityEngine;

public class GhostActivator : MonoBehaviour
{
    // 🌟 --- เพิ่มตัวแปรเซฟความจำตรงนี้ --- 🌟
    [Header("🌟 ชื่อเซฟของผีคลาน (ตั้งให้ไม่ซ้ำกัน!)")]
    public string ghostSaveID = "Crawling_Ghost_1";

    // ลากตัวผีที่ซ่อนอยู่ (Ghostly Woman 2) มาใส่ในช่องนี้
    public GameObject targetGhost;

    // ปรับเวลาให้ผีโชว์ตัวสั้นลงเหลือ 1.9 วินาที (เพื่อให้รับกับความเร็วที่เพิ่มขึ้น)
    public float displayDuration = 1.9f;

    // 🌟 1. ฟังก์ชันนี้ทำงานตอนเริ่มเกม (โหลดเซฟ)
    private void Start()
    {
        // เช็คความจำ: ถ้าเคยเจอผีตัวนี้คลานผ่านไปแล้ว
        if (PlayerPrefs.GetInt(ghostSaveID, 0) == 1)
        {
            // ทำลายตัวผีและกล่องดักทิ้งไปเลยตั้งแต่เริ่มเกม!
            if (targetGhost != null)
            {
                Destroy(targetGhost);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // เช็กว่าสิ่งที่เดินชนคือผู้เล่นที่มี Tag ว่า Player หรือไม่
        if (other.CompareTag("Player"))
        {
            // 🌟 2. ประทับตราเซฟ! จำไว้ว่าผู้เล่นโดนหลอกสำเร็จแล้ว
            PlayerPrefs.SetInt(ghostSaveID, 1);
            PlayerPrefs.Save();

            // 1. สั่งให้ผีปรากฏตัวออกมาคลาน
            if (targetGhost != null) targetGhost.SetActive(true);

            // 2. สั่งให้ทำลายผีทิ้งหลังจากผ่านไป 1.9 วินาที
            if (targetGhost != null) Destroy(targetGhost, displayDuration);

            // 3. ทำลายกล่องกับดักทิ้งทันที เพื่อไม่ให้เกิดเหตุการณ์ซ้ำ
            Destroy(gameObject);
        }
    }
}