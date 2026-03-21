using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI counterText;
    private int currentEvidence = 0;
    private int totalEvidence = 5;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // ดึงข้อมูลจำนวนหลักฐานเดิมที่เคยเซฟไว้
        currentEvidence = PlayerPrefs.GetInt("TotalEvidence", 0);

        // สั่งอัปเดตข้อความทันทีที่เริ่มเกม
        UpdateCounterUI();
    }

    public void AddEvidence()
    {
        currentEvidence++;

        // เซฟตัวเลขจำนวนหลักฐาน
        PlayerPrefs.SetInt("TotalEvidence", currentEvidence);
        PlayerPrefs.Save();

        // อัปเดตข้อความบนหน้าจอ
        UpdateCounterUI();

        if (currentEvidence >= 5)
        {
            Debug.Log("หลักฐานครบ 5 ชิ้นแล้ว! ออกจากบ้านได้...");
        }
    }

    // --- ฟังก์ชันใหม่: แยกส่วนการอัปเดตข้อความ UI เพื่อให้โค้ดสะอาดขึ้น ---
    void UpdateCounterUI()
    {
        if (counterText != null)
        {
            if (currentEvidence < 5)
            {
                // ถ้ายังเก็บไม่ครบ
                counterText.text = "หาหลักฐานภายในบ้าน " + currentEvidence + "/" + totalEvidence;
            }
            else
            {
                // ถ้าเก็บครบ 5 ชิ้นแล้ว
                counterText.text = "ออกจากบ้าน " + currentEvidence + "/" + totalEvidence;
                
                // แถม: เปลี่ยนสีตัวหนังสือเป็นสีเขียวหรือสีเหลืองให้เด่นขึ้นก็ได้นะครับ (ถ้าต้องการ)
                // counterText.color = Color.green; 
            }
        }
    }

    public int GetEvidenceCount()
    {
        return currentEvidence;
    }
}