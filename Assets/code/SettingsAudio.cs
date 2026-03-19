using UnityEngine;
using UnityEngine.Audio;

public class SettingsAudio : MonoBehaviour
{
    public AudioMixer mainMixer; // ช่องสำหรับลาก MainMixer มาใส่

    // ฟังก์ชันสำหรับสไลเดอร์ BGM
    public void SetBGMVolume(float volume)
    {
        mainMixer.SetFloat("BGMVolume", volume);
    }

    // ฟังก์ชันสำหรับสไลเดอร์ SFX
    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVolume", volume);
    }

    // ฟังก์ชันสำหรับ Dropdown กราฟิก (ต่ำ=0, กลาง=1, สูง=2)
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}