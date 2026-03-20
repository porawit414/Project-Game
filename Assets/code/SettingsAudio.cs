using UnityEngine;
using UnityEngine.Audio;

public class SettingsAudio : MonoBehaviour
{
    [Header("ใส่ MainMixer ตรงนี้")]
    public AudioMixer mainMixer;

    // 🎵 สำหรับเสียงดนตรี (BGM)
    public void SetBGMVolume(float sliderValue)
    {
        // คำนวณค่าเสียง
        float dbValue = Mathf.Log10(sliderValue) * 20f;
        // ส่งค่าไปที่ Mixer
        mainMixer.SetFloat("BGM", dbValue);

        // 🚨 เครื่องจับเท็จ BGM
        Debug.Log("🎵 สไลเดอร์ BGM ขยับ! ค่า Slider: " + sliderValue + " | แปลงเป็น: " + dbValue + " dB");
    }

    // 🔊 สำหรับเสียงเอฟเฟกต์ (SFX)
    public void SetSFXVolume(float sliderValue)
    {
        // คำนวณค่าเสียง
        float dbValue = Mathf.Log10(sliderValue) * 20f;
        // ส่งค่าไปที่ Mixer
        mainMixer.SetFloat("SFX", dbValue);

        // 🚨 เครื่องจับเท็จ SFX
        Debug.Log("🔊 สไลเดอร์ SFX ขยับ! ค่า Slider: " + sliderValue + " | แปลงเป็น: " + dbValue + " dB");
    }
}