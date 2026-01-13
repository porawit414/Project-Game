using System.Collections; // *สำคัญ* ต้องมีบรรทัดนี้เพื่อใช้ระบบหน่วงเวลา
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderSystem : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject dialoguePanel;
    public Image leaderFace;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI commandText;

    [Header("Settings")]
    public Sprite leaderSprite;
    public string leaderName = "Headquarters";
    public float typingSpeed = 0.05f; // ความเร็วในการพิมพ์ (ค่ายิ่งน้อยยิ่งเร็ว)

    void Start()
    {
        ClosePanel();
        // ทดสอบ
        ShowCommand("Soldier! Go to the red marker on your map immediately!"); 
    }

    public void ShowCommand(string message)
    {
        dialoguePanel.SetActive(true);
        leaderFace.sprite = leaderSprite;
        nameText.text = leaderName;

        // สั่งให้หยุดพิมพ์อันเก่าก่อน (ถ้ามี) แล้วเริ่มพิมพ์ประโยคใหม่
        StopAllCoroutines();
        StartCoroutine(TypeSentence(message));
    }

    // ฟังก์ชันนี้จะทำหน้าที่ค่อยๆ เติมตัวอักษร
    IEnumerator TypeSentence(string sentence)
    {
        commandText.text = ""; // ล้างข้อความเก่าให้ว่าง
        
        // วนลูปหยิบตัวอักษรมาทีละตัว
        foreach (char letter in sentence.ToCharArray())
        {
            commandText.text += letter; // เติมตัวอักษรเข้าไป
            yield return new WaitForSeconds(typingSpeed); // รอเวลาแป๊บนึง
        }
    }

    public void ClosePanel()
    {
        dialoguePanel.SetActive(false);
    }
}