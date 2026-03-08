using UnityEngine;
using System.Collections; 

public class WavingGhostAudio : MonoBehaviour
{
    [Header("ตั้งค่าเสียงผีโบกมือ")]
    public AudioClip ghostSound; 
    public float soundDelay = 0f; 
    
    private AudioSource audioSource; 

    void Awake()
    {
        // สร้างเครื่องเล่นเสียง
        audioSource = gameObject.AddComponent<AudioSource>(); 
        audioSource.playOnAwake = false; 
        
        // ตั้งค่าระบบเสียง 3 มิติ
        audioSource.spatialBlend = 1f; 
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f; 
        audioSource.maxDistance = 15f; 
    }

    void OnEnable()
    {
        if (ghostSound != null && audioSource != null)
        {
            audioSource.clip = ghostSound; 
            StartCoroutine(PlayDelayedSound());
        }
    }

    IEnumerator PlayDelayedSound()
    {
        yield return new WaitForSeconds(soundDelay);
        
        if (audioSource != null)
        {
            audioSource.Play(); 
        }
    }
}