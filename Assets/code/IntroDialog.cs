using System.Collections;
using UnityEngine;
using TMPro;

public class IntroDialog : MonoBehaviour
{
    // 🚨 ไม้กั้นปุ่ม Spacebar อยู่ตรงนี้ครับ! (บรรทัดนี้แหละที่ขาดไป)
    public static bool isIntroActive = false;

    [Header("ใส่ชิ้นส่วน UI")]
    public GameObject dialogPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;

    [Header("รูปโปรไฟล์หัวหน้า")]
    public GameObject bossProfileImage;

    [Header("จอดำพื้นหลัง")]
    public GameObject blackScreenPanel;

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
        // 🌟 1. เช็คความจำ: เคยดูคำสั่งหัวหน้าไปหรือยัง?
        if (PlayerPrefs.GetInt("HasSeenIntro", 0) == 1)
        {
            isIntroActive = false; // ถ้าเคยดูแล้ว ปลดล็อคปุ่ม Spacebar ทันที

            dialogPanel.SetActive(false);
            if (blackScreenPanel != null) blackScreenPanel.SetActive(false);
            if (bossProfileImage != null) bossProfileImage.SetActive(false);

            AudioListener.pause = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            this.enabled = false;
            return;
        }

        // 🚨 2. บอสกำลังจะพูด! สั่งล็อคปุ่ม Spacebar ในเกมทั้งหมด!
        isIntroActive = true;

        dialogPanel.SetActive(true);
        if (blackScreenPanel != null) blackScreenPanel.SetActive(true);

        AudioListener.pause = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        nameText.text = speakerNames[currentLine];
        messageText.text = "";

        if (bossProfileImage != null)
        {
            if (speakerNames[currentLine] == "หัวหน้า อังเดร ไนท์ฟอร์ด")
            {
                bossProfileImage.SetActive(true);
            }
            else
            {
                bossProfileImage.SetActive(false);
            }
        }

        foreach (char letter in dialogMessages[currentLine].ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
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
            // 🔓 3. บอสพูดจบแล้ว! ปลดล็อคให้กด Spacebar ได้ตามปกติ
            isIntroActive = false;

            dialogPanel.SetActive(false);
            if (bossProfileImage != null) bossProfileImage.SetActive(false);
            if (blackScreenPanel != null) blackScreenPanel.SetActive(false);

            AudioListener.pause = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            PlayerPrefs.SetInt("HasSeenIntro", 1);
            PlayerPrefs.Save();
        }
    }
}