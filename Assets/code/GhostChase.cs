using UnityEngine;
using System.Collections; 

public class GhostChase : MonoBehaviour
{
    [Header("ตั้งค่าการวิ่ง")]
    public Transform player; 
    public float moveSpeed = 0.5f; // ความเร็ววิ่งปัจจุบันคือ 0.5

    [Header("ตั้งค่าเสียงหลอน")]
    public AudioClip scarySound; 
    public float soundDelay = 0f; 
    private AudioSource audioSource; 

    void Start()
    {
        // 1. ค้นหากล้องหลักของผู้เล่น
        if (player == null && Camera.main != null)
        {
            player = Camera.main.transform; 
        }
        else if (player == null) 
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // 2. ตั้งค่าระบบเสียงอัตโนมัติ 
        audioSource = gameObject.AddComponent<AudioSource>(); 
        audioSource.clip = scarySound; 
        audioSource.playOnAwake = false; 
        
        if (scarySound != null)
        {
            StartCoroutine(PlaySoundWithDelay()); 
        }

        // 3. ตั้งเวลาทำลายตัวเอง (ปรับเป็นหายไปใน 1.2 วินาทีแล้วครับ!)
        Destroy(gameObject, 1.4f); 
    }

    IEnumerator PlaySoundWithDelay()
    {
        yield return new WaitForSeconds(soundDelay); 
        
        if (audioSource != null) 
        {
            audioSource.Play(); 
        }
    }

    void Update() 
    {
        if (player != null) 
        {
            transform.LookAt(player.position); 
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
}