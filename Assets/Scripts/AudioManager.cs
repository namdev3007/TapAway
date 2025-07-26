using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    private const string BGM_KEY = "BGM_ON";
    private const string SFX_KEY = "SFX_ON";
    private const string BGM_VOL_KEY = "BGM_VOLUME";
    private const string SFX_VOL_KEY = "SFX_VOLUME";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(int index)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.loop = true;
            if (IsBGMOn()) bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Length && IsSFXOn())
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
    }

    public void ToggleBGM()
    {
        bool on = !IsBGMOn();
        PlayerPrefs.SetInt(BGM_KEY, on ? 1 : 0);
        PlayerPrefs.Save();
        bgmSource.mute = !on;

        if (on && !bgmSource.isPlaying)
            bgmSource.Play();
        else if (!on && bgmSource.isPlaying)
            bgmSource.Pause();
    }

    public void ToggleSFX()
    {
        bool on = !IsSFXOn();
        PlayerPrefs.SetInt(SFX_KEY, on ? 1 : 0);
        PlayerPrefs.Save();
        sfxSource.mute = !on;
    }

    public bool IsBGMOn()
    {
        return PlayerPrefs.GetInt(BGM_KEY, 1) == 1;
    }

    public bool IsSFXOn()
    {
        return PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }

    public void LoadSettings()
    {
        bgmSource.mute = !IsBGMOn();
        sfxSource.mute = !IsSFXOn();

        bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOL_KEY, 1f);
        sfxSource.volume = PlayerPrefs.GetFloat(SFX_VOL_KEY, 1f);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat(BGM_VOL_KEY, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFX_VOL_KEY, volume);
        PlayerPrefs.Save();
    }
}
