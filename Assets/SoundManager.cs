using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;

    public AudioClip matchSound;
    public AudioClip buttonSound;

    public AudioClip rocketSound;
    public AudioClip bombSound;
    public AudioClip colorBombSound;

    public AudioClip victorySound;
    public AudioClip defeatSound;

    private void Awake()
    {
        instance = this;
    }

    public void PlayMatch()
    {
        if (!SettingsManager.instance.soundEnabled) return;
        audioSource.PlayOneShot(matchSound);
    }

    public void PlayButton()
    {
        if (!SettingsManager.instance.soundEnabled) return;
        audioSource.PlayOneShot(buttonSound);
    }

    public void PlayRocket()
    {
        if (!SettingsManager.instance.soundEnabled) return;
        audioSource.PlayOneShot(rocketSound);
    }

    public void PlayBomb()
    {
        if (!SettingsManager.instance.soundEnabled) return;
        audioSource.PlayOneShot(bombSound);
    }

    public void PlayColorBomb()
    {
        if (!SettingsManager.instance.soundEnabled) return;
        audioSource.PlayOneShot(colorBombSound);
    }

    public void PlayVictory()
    {
        if (!SettingsManager.instance.soundEnabled) return;
        audioSource.PlayOneShot(victorySound);
    }

    public void PlayDefeat()
    {
        if (!SettingsManager.instance.soundEnabled) return;
        audioSource.PlayOneShot(defeatSound);
    }
    public void UpdateVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private void Start()
    {
        audioSource.volume = 1f; 
    }
}