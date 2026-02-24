using UnityEngine;

public class GhostChase : MonoBehaviour
{
    public Transform player; 
    public float moveSpeed = 3f; 

    void Start()
    {
        // ให้ผีค้นหาวัตถุที่มี Tag ว่า "Player" ในฉากโดยอัตโนมัติ
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // ตั้งเวลาหายไปใน 0.7 วินาที
        Destroy(gameObject, 0.7f); 
    }

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
}