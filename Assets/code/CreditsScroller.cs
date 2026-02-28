using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [Header("ความเร็วในการเลื่อน")]
    public float scrollSpeed = 50f; 

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // สั่งให้ขยับขึ้นแกน Y (ด้านบน) เรื่อยๆ
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}