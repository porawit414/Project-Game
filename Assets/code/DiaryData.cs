using UnityEngine;

[CreateAssetMenu(fileName = "NewDiary", menuName = "HorrorGame/Diary Data")]
public class DiaryData : ScriptableObject
{
    public string diaryName; // ชื่อหัวข้อ
    [TextArea(5, 10)]
    public string diaryContent; // เนื้อหาข้างใน (TextArea ทำให้มีกล่องพิมพ์ข้อความใหญ่ขึ้นใน Inspector)
    // public Sprite diaryImage; // ถ้าอยากให้มีรูปภาพแปะด้วย ปลดคอมเมนต์อันนี้
}