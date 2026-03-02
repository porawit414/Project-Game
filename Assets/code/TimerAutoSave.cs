using UnityEngine;

public class TimerAutoSave : MonoBehaviour
{
    [Header("ตัวละคร Player (ลากมาใส่)")]
    public GameObject playerObject;

    [Header("เวลาที่จะให้เซฟ (หน่วยเป็นวินาที)")]
    public float saveInterval = 300f; // 300 วินาที = 5 นาทีพอดีเป๊ะ!

    private float timer = 0f;

    void Update()
    {
        // ถ้าไม่ได้ใส่ตัวละครไว้ ให้หยุดทำงานจะได้ไม่พัง
        if (playerObject == null) return;

        // นับเวลาเพิ่มขึ้นเรื่อยๆ ตามเวลาจริงในเกม
        timer += Time.deltaTime;

        // เช็คว่าถึงเวลาที่กำหนดหรือยัง (ครบ 5 นาที)
        if (timer >= saveInterval)
        {
            AutoSave();

            // รีเซ็ตนาฬิกากลับเป็น 0 เพื่อเริ่มนับ 5 นาทีรอบใหม่
            timer = 0f;
        }
    }

    void AutoSave()
    {
        // แอบจดจำตำแหน่ง X, Y, Z ปัจจุบันของผู้เล่นลงเครื่อง
        PlayerPrefs.SetFloat("SavedPlayerX", playerObject.transform.position.x);
        PlayerPrefs.SetFloat("SavedPlayerY", playerObject.transform.position.y);
        PlayerPrefs.SetFloat("SavedPlayerZ", playerObject.transform.position.z);

        // สั่งให้เซฟลงเครื่องเดี๋ยวนี้!
        PlayerPrefs.Save();

        Debug.Log("⏱️ [Auto Save] เซฟตำแหน่งอัตโนมัติตามเวลาเรียบร้อย!");
    }
}