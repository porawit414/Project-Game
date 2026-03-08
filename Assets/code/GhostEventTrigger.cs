using UnityEngine;

public class GhostEventTrigger : MonoBehaviour
{
    [Header("ใส่ตัวผี (Ghost_Waving) ลงในช่องนี้")]
    public GameObject ghostObject;

    [Header("ตั้งค่าการทำงานของกล่องเซนเซอร์")]
    [Tooltip("ติ๊กถูก = จุดให้ผีโผล่ / เอาออก = จุดให้ผีหาย")]
    public bool isShowTrigger = true; 

    [Tooltip("ติ๊กถูก = ผู้เล่นต้องเก็บมีดก่อน ผีถึงจะโผล่ได้")]
    public bool requireKnife = false; 

    // 👉 ตัวแปรนี้แหละครับที่ระบบแจ้งว่าหาไม่เจอ ตอนนี้เราเติมให้แล้ว!
    public static bool hasPickedUpKnife = false; 

    private bool isUsed = false;

    void OnTriggerEnter(Collider other)
    {
        if (isUsed) return; 

        if (other.CompareTag("Player"))
        {
            if (isShowTrigger && requireKnife && !hasPickedUpKnife)
            {
                return; 
            }

            if (ghostObject != null)
            {
                ghostObject.SetActive(isShowTrigger);
                isUsed = true; 
            }
        }
    }
}