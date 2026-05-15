using TMPro;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public bool soundEnabled = true;
    public bool musicEnabled = true;
    public bool hintsEnabled = true;

    public TextMeshProUGUI soundText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI hintsText;

    private void Start()
    {
        UpdateTexts();
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