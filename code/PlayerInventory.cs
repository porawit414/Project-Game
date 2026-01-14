using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // เก็บรหัสกุญแจที่เราได้แล้ว เช่น "Room1"
    private HashSet<string> keys = new HashSet<string>();

    public void AddKey(string keyId)
    {
        keys.Add(keyId);
        Debug.Log("Got key: " + keyId);
    }

    public bool HasKey(string keyId)
    {
        return keys.Contains(keyId);
    }
}
