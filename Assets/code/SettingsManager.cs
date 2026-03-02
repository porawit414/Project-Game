using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // โหลดค่าเสียงเดิมที่เคยเซฟไว้
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            AudioListener.volume = savedVolume;
            if (volumeSlider != null) volumeSlider.value = savedVolume;
        }
        else
        {
            AudioListener.volume = 1f;
            if (volumeSlider != null) volumeSlider.value = 1f;
        }
    }

    // ฟังก์ชันนี้จะถูกเรียกตอนเราเอามือเลื่อนหลอด
    public void SetMasterVolume(float sliderValue)
    {
        AudioListener.volume = sliderValue; // ปรับเสียงเกม
        PlayerPrefs.SetFloat("MasterVolume", sliderValue); // เซฟค่าไว้
        PlayerPrefs.Save();
    }
}