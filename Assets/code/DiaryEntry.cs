using UnityEngine;

[CreateAssetMenu(fileName = "New Diary", menuName = "HorrorGame/Diary Entry")]
public class DiaryEntry : ScriptableObject
{
    public string diaryTitle;
    [TextArea(5, 10)] // ทำให้ช่องกรอกข้อความใน Inspector ใหญ่ขึ้น
    public string diaryText;
    public Sprite diaryImage; // ถ้ามีรูปประกอบ
}