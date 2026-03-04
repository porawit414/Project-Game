using UnityEngine;

public class SpookyTreeWind : MonoBehaviour
{
    [Header("ตั้งค่าความหลอนของลม")]
    public float windSpeed = 1.5f;   // ความเร็วของลม (ยิ่งเยอะยิ่งโยกเร็ว)
    public float swayAmount = 2.0f;  // ความแรงในการโยก (องศา)

    private Quaternion startRotation;

    void Start()
    {
        // จำตำแหน่งองศาเริ่มต้นไว้
        startRotation = transform.rotation;
    }

    void Update()
    {
        // คำนวณการแกว่งโดยใช้คลื่นคณิตศาสตร์ (Sine Wave)
        float swayX = Mathf.Sin(Time.time * windSpeed) * swayAmount;
        float swayZ = Mathf.Cos(Time.time * windSpeed * 0.8f) * (swayAmount * 0.5f);

        // สั่งให้ต้นไม้หมุนโยกไปมา
        transform.rotation = startRotation * Quaternion.Euler(swayX, 0, swayZ);
    }
}