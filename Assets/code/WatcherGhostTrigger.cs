using UnityEngine;
using System.Collections;

// บรรทัดนี้จะบังคับให้ Unity สร้างลำโพง (AudioSource) ใส่กล่องนี้ให้อัตโนมัติเลยครับ
[RequireComponent(typeof(AudioSource))] 
public class WatcherGhostTrigger : MonoBehaviour 
{
    [Header("Watcher Setup")]
    public GameObject watcherObject;    
    public GameObject watcherModel;     
    
    [Header("Audio Settings")]
    public AudioSource audioSource;   
    public AudioClip jumpscareSound;    

    [Header("Settings")]
    public float vanishTime = 1.0f;     

    private bool isGhostActive = false;
    private bool hasScreamed = false;
    private Renderer rd; // ย้ายมาประกาศตรงนี้เพื่อเก็บค่าไว้ใช้ยาวๆ

    // ฟังก์ชันนี้ทำงานตอนกด Play เกม (เตรียมของไว้ล่วงหน้า จะได้ไม่กระตุก)
    void Start()
    {
        // 1. หาภาพผีเตรียมไว้
        if (watcherModel != null) 
        {
            rd = watcherModel.GetComponent<Renderer>();
        }

        // 2. เตรียมลำโพงและเสียงไว้ล่วงหน้า
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        if (audioSource != null && jumpscareSound != null)
        {
            audioSource.clip = jumpscareSound;
            audioSource.playOnAwake = false; // ป้องกันเสียงร้องเองตอนเริ่มเกม
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && !isGhostActive) 
        {
            isGhostActive = true;
            
            if (watcherObject != null) watcherObject.SetActive(true);
            
            StartCoroutine(CheckIfPlayerSeesWatcher());
        }
    }

    IEnumerator CheckIfPlayerSeesWatcher()
    {
        while (!hasScreamed)
        {
            if (rd != null && IsVisibleToCamera(rd)) 
            {
                hasScreamed = true;
                
                // สั่งเล่นเสียงที่เตรียมไว้แล้วได้เลยทันที (ลื่นแน่นอน)
                if (audioSource != null)
                {
                    audioSource.Play();
                }

                yield return new WaitForSeconds(vanishTime);
                
                if (watcherObject != null) watcherObject.SetActive(false);
                
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
                
                Destroy(gameObject, 0.5f); 
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private bool IsVisibleToCamera(Renderer renderer)
    {
        if (Camera.main == null) return false;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}