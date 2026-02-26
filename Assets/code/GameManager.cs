using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI counterText; 
    private int currentEvidence = 0; // ในสคริปต์ประตูใช้ชื่อ evidenceCount หรือ currentEvidence เช็คให้ตรงกันนะครับ
    private int totalEvidence = 5;

    void Awake()
    {
        instance = this;
    }

    public void AddEvidence()
    {
        currentEvidence++;
        counterText.text = currentEvidence + "/" + totalEvidence;
        
        if (currentEvidence >= 5)
        {
            Debug.Log("หลักฐานครบ 5 ชิ้นแล้ว! เกมใกล้จบ...");
        }
    }

    // === ก๊อปปี้ส่วนนี้ไปเพิ่มครับ ===
    public int GetEvidenceCount()
    {
        return currentEvidence;
    }
    // ===========================
}