using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float rotateSpeed = 100f; // ปรับความเร็วหมุนตรงนี้

    void Update()
    {
        // สั่งให้หมุนรอบแกน Y (แนวตั้ง) อย่างเดียว
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}