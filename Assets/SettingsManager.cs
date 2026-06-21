using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public bool soundEnabled = true;
    public bool musicEnabled = true;
    public bool hintsEnabled = true;

    public TextMeshProUGUI soundText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI hintsText;

    public static SettingsManager instance;
    
    public float soundVolume = 1f;
    public Slider volumeSlider; 
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LoadSettings();
        UpdateTexts();
    }
    void LoadSettings()
    {
        soundEnabled = PlayerPrefs.GetInt("Sound", 1) == 1;
        musicEnabled = PlayerPrefs.GetInt("Music", 1) == 1;
        hintsEnabled = PlayerPrefs.GetInt("Hints", 1) == 1;
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);

        if (volumeSlider != null)
            volumeSlider.value = soundVolume * 10f; // переводим 0-1 обратно в 0-10
    }
    public void OnVolumeChanged(float value)
    {
        Debug.Log("RAW slider value: " + value);
        soundVolume = value / 10f;
        Debug.Log("Calculated soundVolume: " + soundVolume);
        SoundManager.instance.UpdateVolume(soundVolume);
    }
    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        UpdateTexts();
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        UpdateTexts();
    }

    public void ToggleHints()
    {
        hintsEnabled = !hintsEnabled;
        UpdateTexts();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Sound", soundEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Music", musicEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Hints", hintsEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.Save();

        Debug.Log("Налаштування збережено");
    }

    void UpdateTexts()
    {
        soundText.text = "Звук: " + (soundEnabled ? "Увімкнено" : "Вимкнено");
        musicText.text = "Музика: " + (musicEnabled ? "Увімкнено" : "Вимкнено");
        hintsText.text = "Підказки: " + (hintsEnabled ? "Увімкнено" : "Вимкнено");
    }
}