using UnityEngine;

public class MyFlashlight : MonoBehaviour
{
    [Header("ลากตัว Flashlight เองมาใส่ตรงนี้")]
    public Light flashlightComponent; // เปลี่ยนจาก GameObject เป็น Light

    private bool isOn = false;

    void Start()
    {
        // เริ่มเกมปุ๊บ สั่งปิดแสงทันที!
        isOn = false;
        if (flashlightComponent != null)
        {
            flashlightComponent.enabled = false; // ปิดแค่แสง (สคริปต์ยังทำงานต่อ)
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isOn = !isOn;
            if (flashlightComponent != null)
            {
                flashlightComponent.enabled = isOn; // สั่งเปิด/ปิดแสง
            }
        }
    }
}