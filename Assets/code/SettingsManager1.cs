using UnityEngine;
using TMPro; // 🌟 สำคัญมาก! ต้องบรรทัดนี้เพื่อเรียกใช้ TextMeshPro

public class SettingsManager : MonoBehaviour
{
    [Header("ลากตัวหนังสือ (TextMeshPro) ที่แสดงเลข 100 มาใส่")]
    public TextMeshProUGUI volumeText; // 🌟 เปลี่ยนมาใช้ TextMeshProUGUI แล้ว!

    private int currentVolume = 100;

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolumeInt"))
        {
            currentVolume = PlayerPrefs.GetInt("MasterVolumeInt");
        }
        else
        {
            currentVolume = 100;
        }
        UpdateVolume();
    }

    public void IncreaseVolume()
    {
        currentVolume += 10;
        if (currentVolume > 100) currentVolume = 100;
        UpdateVolume();
    }

    public void DecreaseVolume()
    {
        currentVolume -= 10;
        if (currentVolume < 0) currentVolume = 0;
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        if (volumeText != null) volumeText.text = currentVolume.ToString();

        float floatVolume = currentVolume / 100f;
        AudioListener.volume = floatVolume;

        PlayerPrefs.SetInt("MasterVolumeInt", currentVolume);
        PlayerPrefs.Save();
    }
}