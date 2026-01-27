using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    void LateUpdate()
    {
        // สั่งให้หมุนตามกล้องหลักตลอดเวลา
        transform.rotation = Camera.main.transform.rotation;
    }
}