using UnityEngine;

public class ArmPoseFix : MonoBehaviour
{
    [Header("ลากกระดูกมาใส่ตรงนี้")]
    public Transform rightArm;      // ต้นแขน (RightArm)
    public Transform rightForeArm;  // ข้อศอก/แขนล่าง (RightForeArm)

    [Header("ปรับองศาแขน (ปรับตอนกด Play ได้เลย)")]
    public Vector3 armAngle = new Vector3(70, 45, 0);       // มุมต้นแขน
    public Vector3 foreArmAngle = new Vector3(0, 0, 0);     // มุมข้อศอก

    // LateUpdate จะทำงานหลัง Animation (ใช้ทับท่า T-Pose)
    void LateUpdate()
    {
        if (rightArm != null)
        {
            rightArm.localEulerAngles = armAngle;
        }

        if (rightForeArm != null)
        {
            rightForeArm.localEulerAngles = foreArmAngle;
        }
    }
}