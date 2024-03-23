using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    #region Use Volume Number

    [Header("ValueNumber")]
    [SerializeField] private TextMeshProUGUI textMusicVolume;
    [SerializeField] private TextMeshProUGUI textEfxVolume;
    private float volumeMusic = 10;
    private float volumeEfx = 10;


    #endregion

    void Start()
    {
        LoadVolumeSettingsNumber();
    }
    private void LoadVolumeSettingsNumber()
    {
        LoadMusicVolumeNumber();
        LoadEfxVolumeNumber();
    }
    private void LoadMusicVolumeNumber()
    {
        float savedVolume = PlayerPrefs.GetFloat(Key.KEY_MUSIC, 1f);
        volumeMusic = Mathf.Round(savedVolume * 10);

        SetVolumeNumber(Key.KEY_MUSIC, volumeMusic, textMusicVolume);
    }
    private void LoadEfxVolumeNumber()
    {
        float savedVolume = PlayerPrefs.GetFloat(Key.KEY_EFX, 1f);
        volumeEfx = Mathf.Round(savedVolume * 10);
        SetVolumeNumber(Key.KEY_EFX, volumeEfx, textEfxVolume);
    }

    private void SetVolumeNumber(string key, float volume, TMP_Text textVolume)
    {
        if (textVolume != null)
        {
            float normalizedVolume = volume / 10f;

            if (volume == 0f)
            {
                normalizedVolume = 0.0001f;
            }

            SoundManager.Instance.SetVolume(key, key, normalizedVolume);
            UpdateTextVolume(textVolume, volume);

        }
    }
    private void UpdateTextVolume(TMP_Text textVolume, float volume)
    {
        textVolume.text = volume.ToString("F0");
    }

    public void AdjustEfxVolumeNumber(float delta)
    {
        SoundManager.Instance.OnClickSound();

        int currentVolume = Mathf.RoundToInt(volumeEfx);
        currentVolume = Mathf.Clamp(currentVolume + Mathf.RoundToInt(delta), 0, 10);
        volumeEfx = currentVolume;
        SetVolumeNumber(Key.KEY_EFX, volumeEfx, textEfxVolume);
    }

    public void AdjustMusicVolumeNumber(float delta)
    {
        SoundManager.Instance.OnClickSound();

        int currentVolume = Mathf.RoundToInt(volumeMusic);
        currentVolume = Mathf.Clamp(currentVolume + Mathf.RoundToInt(delta), 0, 10);
        volumeMusic = currentVolume;
        SetVolumeNumber(Key.KEY_MUSIC, volumeMusic, textMusicVolume);
    }


}
