using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;

    [SerializeField] private string musicVolumeParameter = "MusicVolume";

    [SerializeField] private string sfxVolumeParameter = "SfxVolume";
    [SerializeField] private Slider musicSlider;

    [SerializeField] private Slider sfxSlider;

    void Start()
    {

        float savedMusicVolume = PlayerPrefs.GetFloat(musicVolumeParameter, 0.75f);
        musicSlider.value = savedMusicVolume;
        SetMusicVolume(savedMusicVolume);


        float savedSfxVolume = PlayerPrefs.GetFloat(sfxVolumeParameter, 0.75f);
        sfxSlider.value = savedSfxVolume;
        SetSfxVolume(savedSfxVolume);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }


    public void SetMusicVolume(float volume)
    {
        float mixerVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        myMixer.SetFloat(musicVolumeParameter, mixerVolume);

        PlayerPrefs.SetFloat(musicVolumeParameter, volume);
        PlayerPrefs.Save();
    }


    public void SetSfxVolume(float volume)
    {
        float mixerVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        myMixer.SetFloat(sfxVolumeParameter, mixerVolume);

        PlayerPrefs.SetFloat(sfxVolumeParameter, volume);
        PlayerPrefs.Save();
    }
}

