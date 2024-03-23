using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource effectAudio;

    [Header("Audio Clips")]
    [Header("UI")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip resultSound;
    [SerializeField] private AudioClip newBestScoreSound;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip noMoneySound;
    [SerializeField] private AudioClip wrongChoiceSound;
    [SerializeField] private AudioClip trueChoiceSound;
    [SerializeField] private AudioClip warningSound;
    [SerializeField] private AudioClip startCountdownSound;
    [SerializeField] private AudioClip timeupCountdownSound;
    
    [Header("BACKGROUND MUSIC")]
    [SerializeField] private AudioClip mainSound;
    [SerializeField] private AudioClip playSound;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey(Key.KEY_MUSIC))
            LoadAllSoundSetting();
        else
            SetDefaultVolume();
    }
    #region Setting

    public void SetVolume(string mixerParameter, string playerPrefsKey, float volume)
    {
        mainMixer.SetFloat(mixerParameter, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(playerPrefsKey, volume);

        switch (mixerParameter)
        {
            case Key.KEY_MUSIC:
                musicAudio.volume = volume;
                break;
            case Key.KEY_EFX:
                effectAudio.volume = volume;
                break;
        }
    }
    private void LoadAllSoundSetting()
    {
        float musicVolume = PlayerPrefs.GetFloat(Key.KEY_MUSIC);
        float sfxVolume = PlayerPrefs.GetFloat(Key.KEY_EFX);

        mainMixer.SetFloat(Key.KEY_MUSIC, Mathf.Log10(musicVolume) * 20);
        mainMixer.SetFloat(Key.KEY_EFX, Mathf.Log10(sfxVolume) * 20);

        musicAudio.volume = musicVolume;
        effectAudio.volume = sfxVolume;
    }


    private void SetDefaultVolume()
    {
        float initialVolume = 1f;
        string[] parameters = { Key.KEY_MUSIC, Key.KEY_EFX };

        foreach (string parameter in parameters)
        {
            SetVolume(parameter, parameter, initialVolume); 
        }

        PlayerPrefs.Save();
    }



    public void PlaySoundEffect(AudioClip clip)
    {
        effectAudio.clip = clip;
        effectAudio.PlayOneShot(clip);
    }
    public void StopSoundEffect()
    {
        effectAudio.Stop();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudio.clip = clip;
        musicAudio.Play();

    }
    public void ChangeBackgroundMusic(AudioClip newClip)
    {
        musicAudio.Stop();
        musicAudio.clip = newClip;
        musicAudio.Play();
    }
    public void StopBackgroundMusic()
    {
        musicAudio.Stop();
    }

    #endregion


    #region Sound

    public void OnGamePlaySound()
    {
        ChangeBackgroundMusic(playSound);
    }
    public void OnMainSound()
    {
        ChangeBackgroundMusic(mainSound);
    }
    public void OnClickSound()
    {
        PlaySoundEffect(clickSound);
    }
    public void OnBuySound()
    {
        PlaySoundEffect(buySound);
    }
    public void OnNoMoneySound()
    {
        PlaySoundEffect(noMoneySound);
    }
    public void OnTrueChoiceSound()
    {
        PlaySoundEffect(trueChoiceSound);
    }
    public void OnWrongChoiceSound()
    {
        PlaySoundEffect(wrongChoiceSound);
    }
    public void OnWarningSound()
    {
        PlaySoundEffect(warningSound);
    }
    public void OnShowResultSound()
    {
        StopSoundEffect();
        PlaySoundEffect(resultSound);
    }
    public void OnNewBestScoreSound()
    {
        StopSoundEffect();
        PlaySoundEffect(newBestScoreSound);
    }
    public void OnPlayCountdownSound()
    {
        PlaySoundEffect(startCountdownSound);
    }
    public void OnPlayTimeupCountdownSound()
    {
        PlaySoundEffect(timeupCountdownSound);
    }

    #endregion
}
