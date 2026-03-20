using UnityEngine;

public class TimerAutoSave : MonoBehaviour
{
    [Header("ตัวละคร Player (ลากมาใส่)")]
    public GameObject playerObject;

    [Header("เวลาที่จะให้เซฟ (หน่วยเป็นวินาที)")]
    public float saveInterval = 300f;

    private float timer = 0f;

    void Start()
    {
        if (playerObject == null) return;

        // 2. เปิดอ่านจดหมายจากหน้าเมนู (0 = เริ่มใหม่, 1 = เล่นต่อ)
        int isLoadGame = PlayerPrefs.GetInt("IsLoadGame", 0);

        if (isLoadGame == 1)
        {
            // 🟢 ถ้าเมนูบอกให้เล่นต่อ -> ดึงตำแหน่งที่เซฟไว้มาใช้
            if (PlayerPrefs.HasKey("SavedPlayerX"))
            {
                float x = PlayerPrefs.GetFloat("SavedPlayerX");
                float y = PlayerPrefs.GetFloat("SavedPlayerY");
                float z = PlayerPrefs.GetFloat("SavedPlayerZ");

                playerObject.transform.position = new Vector3(x, y, z);
                Debug.Log("🔄 โหลดเซฟสำเร็จ: เล่นต่อจากจุดเดิม!");
            }
        }
        else
        {
            // 🔴 ถ้าเมนูบอกให้เริ่มใหม่ -> ไม่ต้องย้ายตำแหน่ง ปล่อยให้เกิดที่จุดเริ่มต้น
            Debug.Log("🆕 เริ่มเกมใหม่: ตัวละครอยู่ที่จุดเกิดดั้งเดิม!");
        }
    }

    void Update()
    {
        if (playerObject == null) return;

        timer += Time.deltaTime;
        if (timer >= saveInterval)
        {
            AutoSave();
            timer = 0f;
        }
    }

    void AutoSave()
    {
        // บันทึกตำแหน่ง X Y Z ลงเครื่อง
        PlayerPrefs.SetFloat("SavedPlayerX", playerObject.transform.position.x);
        PlayerPrefs.SetFloat("SavedPlayerY", playerObject.transform.position.y);
        PlayerPrefs.SetFloat("SavedPlayerZ", playerObject.transform.position.z);

        // 🌟 สำคัญมาก: สร้างกุญแจบอกหน้าเมนูว่า "มีเซฟแล้วนะ โชว์ปุ่มเริ่มใหม่ได้เลย!"
        PlayerPrefs.SetInt("HasSave", 1);
        PlayerPrefs.Save();

        Debug.Log("⏱️ ออโต้เซฟทำงานเรียบร้อย!");
    }
}