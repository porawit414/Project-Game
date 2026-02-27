using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    [Header("ลาก NotificationText จาก Canvas มาใส่")]
    public TextMeshProUGUI notificationText;

    [Header("ลาก NotificationText ตัวเดิม (ที่ใส่ Canvas Group แล้ว) มาใส่ช่องนี้ด้วย")]
    public CanvasGroup canvasGroup;

    [Header("ตั้งค่าเวลา")]
    public float displayTime = 3f;
    public float fadeDuration = 0.5f; // เวลาที่ใช้ค่อยๆ สว่าง/จาง

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        // เริ่มเกมมา ให้โปร่งใส 100% (ซ่อนไว้)
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }

    public void ShowNotification(string itemName)
    {
        StopAllCoroutines();
        StartCoroutine(FadeSequence(itemName));
    }

    IEnumerator FadeSequence(string itemName)
    {
        notificationText.text = "เก็บหลักฐาน: " + itemName;

        // 1. ค่อยๆ สว่างขึ้น (Fade In)
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        if (canvasGroup != null) canvasGroup.alpha = 1f;

        // 2. โชว์ค้างไว้ตามเวลา displayTime
        yield return new WaitForSeconds(displayTime);

        // 3. ค่อยๆ จางหายไป (Fade Out)
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }
}