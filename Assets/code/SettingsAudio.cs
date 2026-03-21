using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    [Header("ใส่ MainMixer ตรงนี้")]
    public AudioMixer mainMixer;

    [Header("ลากหลอด Slider มาใส่ตรงนี้")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        // โหลดความจำเสียง
        float savedBGM = PlayerPrefs.GetFloat("SavedBGM", 1f);
        float savedSFX = PlayerPrefs.GetFloat("SavedSFX", 1f);

        // เซ็ตค่าให้ Slider
        if (bgmSlider != null) bgmSlider.value = savedBGM;
        if (sfxSlider != null) sfxSlider.value = savedSFX;

        SetBGMVolume(savedBGM);
        SetSFXVolume(savedSFX);
    }

    // 🎵 สำหรับเสียงดนตรี (BGM)
    public void SetBGMVolume(float sliderValue)
    {
        float dbValue = Mathf.Log10(sliderValue) * 20f;
        mainMixer.SetFloat("BGMVolume", dbValue);

        PlayerPrefs.SetFloat("SavedBGM", sliderValue);
        PlayerPrefs.Save();
    }

    // 🔊 สำหรับเสียงเอฟเฟกต์ (SFX)
    public void SetSFXVolume(float sliderValue)
    {
        float dbValue = Mathf.Log10(sliderValue) * 20f;
        mainMixer.SetFloat("SFXVolume", dbValue);

        PlayerPrefs.SetFloat("SavedSFX", sliderValue);
        PlayerPrefs.Save();
    }

    // ==========================================
    // 🖥️ ฟังก์ชันใหม่! สำหรับปรับกราฟิก (เพิ่มเข้ามาตรงนี้)
    // ==========================================
    public void SetQuality(int qualityIndex)
    {
        // สั่ง Unity ให้เปลี่ยนระดับกราฟิก (0 = ต่ำสุด, 1 = กลาง, 2 = สูง ...)
        QualitySettings.SetQualityLevel(qualityIndex);

        // 🚨 เครื่องจับเท็จ (ให้มันเด้งบอกใน Console ว่าเปลี่ยนแล้วจริงๆ)
        Debug.Log("🖥️ เปลี่ยนกราฟิกเป็นระดับ: " + qualityIndex);
    }
}