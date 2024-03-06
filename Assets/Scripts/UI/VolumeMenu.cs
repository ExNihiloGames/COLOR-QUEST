using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeMenu : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    public static Action<float> onMasterVolumeChanged;

    [SerializeField] Slider effectsSlider;
    public static Action<float> onEffectsVolumeChanged;

    [SerializeField] Slider musicSlider;
    public static Action<float> onMusicVolumeChanged;

    [SerializeField] Slider menusSlider;
    public static Action<float> onMenusVolumeChanged;


    private void OnEnable()
    {
        masterSlider.value = PlayerPrefs.GetFloat(SoundManager.masterVolumeParamName, 1);
        masterSlider.onValueChanged.AddListener((value) => onMasterVolumeChanged?.Invoke(value));

        effectsSlider.value = PlayerPrefs.GetFloat(SoundManager.effectsVolumeParamName, 1);
        effectsSlider.onValueChanged.AddListener((value) => onEffectsVolumeChanged?.Invoke(value));

        musicSlider.value = PlayerPrefs.GetFloat(SoundManager.musicVolumeParamName, 1);
        musicSlider.onValueChanged.AddListener((value) => onMusicVolumeChanged?.Invoke(value));

        menusSlider.value = PlayerPrefs.GetFloat(SoundManager.menusVolumeParamName, 1);
        menusSlider.onValueChanged.AddListener((value) => onMenusVolumeChanged?.Invoke(value));
    }

    private void OnDisable()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        effectsSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        menusSlider.onValueChanged.RemoveAllListeners();
    }
}
