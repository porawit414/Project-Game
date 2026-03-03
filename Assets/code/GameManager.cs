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
        // 🌟 1. ดึงข้อมูลจำนวนหลักฐานเดิมที่เคยเซฟไว้ (ถ้าเพิ่งเริ่มเกมใหม่จะเป็น 0)
        currentEvidence = PlayerPrefs.GetInt("TotalEvidence", 0);

        // สั่งอัปเดตตัวเลขบนหน้าจอให้ตรงกับเซฟทันทีที่โหลดฉาก
        if (counterText != null)
        {
            counterText.text = currentEvidence + "/" + totalEvidence;
        }
    }

    public void AddEvidence()
    {
        currentEvidence++;

        if (counterText != null)
        {
            counterText.text = currentEvidence + "/" + totalEvidence;
        }

        // 🌟 2. เซฟตัวเลขจำนวนหลักฐานอัปเดตล่าสุดลงในเครื่อง
        PlayerPrefs.SetInt("TotalEvidence", currentEvidence);
        PlayerPrefs.Save();

        if (currentEvidence >= 5)
        {
            Debug.Log("หลักฐานครบ 5 ชิ้นแล้ว! เกมใกล้จบ...");
        }
    }

    // เอาไว้ให้สคริปต์อื่น (เช่น ประตู) มาขอดูว่าหลักฐานครบหรือยัง
    public int GetEvidenceCount()
    {
        return currentEvidence;
    }
}