using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string characterName;    // ชื่อตัวละคร
    public Sprite characterPortrait; // รูปภาพหน้าตัวละคร
    
    [TextArea(3, 10)] // ทำให้ช่องพิมพ์ใน Inspector กว้างขึ้น
    public string[] sentences;      // ชุดประโยคที่จะพูด
}