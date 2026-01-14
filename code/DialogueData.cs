using System;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueSide { Left, Right }

[CreateAssetMenu(fileName = "DialogueData", menuName = "Game/Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines = new();
}

[Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 6)] public string text;

    public DialogueSide side = DialogueSide.Left;
    public Sprite portrait; // ใส่รูปของคนพูดในบรรทัดนี้
}
