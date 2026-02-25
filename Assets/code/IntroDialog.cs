using System.Collections;
using UnityEngine;
using TMPro; // 🌟 เพิ่มบรรทัดนี้ เพื่อเรียกใช้ระบบ TextMeshPro

public class IntroDialog : MonoBehaviour
{
    [Header("ใส่ชิ้นส่วน UI")]
    public GameObject dialogPanel;
    public TextMeshProUGUI nameText;    // 🌟 เปลี่ยนจาก Text เป็น TextMeshProUGUI
    public TextMeshProUGUI messageText; // 🌟 เปลี่ยนจาก Text เป็น TextMeshProUGUI

    [Header("ตั้งค่าความเร็ว")]
    public float typingSpeed = 0.05f;

    private string[] speakerNames = {
        "หัวหน้า อังเดร ไนท์ฟอร์ด",
        "จอน คิมสัน"
    };

    private string[] dialogMessages = {
        "คุณนักสืบ จอน คิมสัน วันนี้คุณได้รับภารกิจให้ไปสืบค้นหาหลักฐานคดีฆาตกรรมในบ้านร้างกลางป่า",
        "รับทราบครับ"
    };

    private int currentLine = 0;
    private bool isTyping = false;

    void Start()
    {
        dialogPanel.SetActive(true);
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        nameText.text = speakerNames[currentLine];
        messageText.text = "";

        foreach (char letter in dialogMessages[currentLine].ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void Update()
    {
        if (dialogPanel.activeSelf && (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0)))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                messageText.text = dialogMessages[currentLine];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogMessages.Length)
        {
            StartCoroutine(TypeSentence());
        }
        else
        {
            dialogPanel.SetActive(false);
        }
    }
}