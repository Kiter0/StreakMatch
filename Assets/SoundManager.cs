using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;

    public AudioClip clickSound;
    public AudioClip matchSound;
    public AudioClip bonusSound;
    public AudioClip buttonSound;

    private void Awake()
    {
        instance = this;
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayMatch()
    {
        audioSource.PlayOneShot(matchSound);
    }

    public void PlayBonus()
    {
        audioSource.PlayOneShot(bonusSound);
    }

    public void PlayButton()
    {
        audioSource.PlayOneShot(buttonSound);
    }
}