using UnityEngine;

public class HandSway : MonoBehaviour
{
    [Header("ความแรงในการแกว่ง")]
    public float amount = 0.02f;    // แกว่งเยอะแค่ไหน
    public float maxAmount = 0.06f; // ขอบเขตสูงสุดที่จะแกว่ง
    public float smoothAmount = 6f; // ความนุ่มนวล (ยิ่งเยอะยิ่งหนืด)

    private Vector3 initialPosition; // ตำแหน่งเริ่มต้นของมือ

    void Start()
    {
        // จำตำแหน่งมือตอนเริ่มเกมไว้
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // รับค่าการขยับเมาส์
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;

        // จำกัดขอบเขตไม่ให้แกว่งหลุดจอ
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        // คำนวณตำแหน่งใหม่
        Vector3 finalPosition = new Vector3(movementX, movementY, 0) + initialPosition;

        // สั่งให้มือขยับไปหาตำแหน่งใหม่อย่างนุ่มนวล
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, Time.deltaTime * smoothAmount);
    }
}