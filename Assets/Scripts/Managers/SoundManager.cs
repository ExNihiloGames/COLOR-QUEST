using System;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class SoundBank
{
    [Serializable]
    public class Sound
    {
        public AudioClip clip => _clip;
        [SerializeField] AudioClip _clip;
        public AudioMixerGroup mixerGroup => _mixerGroup;
        [SerializeField] AudioMixerGroup _mixerGroup;
    }
    public Sound swapColorSound => _swapColorSound;
    [SerializeField] private Sound _swapColorSound;
    public Sound lockedColorSound => _lockedColorSound;
    [SerializeField] private Sound _lockedColorSound;
    public Sound translationSound => _translationSound;
    [SerializeField] private Sound _translationSound;

    public Sound endlevelSound => _endlevelSound;
    [SerializeField] private Sound _endlevelSound;
}

public class SoundManager : MonoBehaviour
{
    public SoundBank soundBank;
    private AudioSource defaultAudioSource;

    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }

    [Header("Audio control")]
    [SerializeField] private AudioMixer audioMixer = default;

    [Range(0.0001f, 1f)]
    [SerializeField] private float _masterVolume = 1f;
    
    public static readonly string masterVolumeParamName = "MasterVolume";
    [Range(0.0001f, 1f)]
    [SerializeField] private float _sfxVolume = 1f;
    public static readonly string effectsVolumeParamName = "FXVolume";
    [Range(0.0001f, 1f)]
    [SerializeField] private float _musicVolume = 1f;
    public static readonly string musicVolumeParamName = "MusicVolume";
    [Range(0.0001f, 1f)]
    [SerializeField] private float _menusVolume = 1f;
    public static readonly string menusVolumeParamName = "UIVolume";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        defaultAudioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        _masterVolume = PlayerPrefs.GetFloat(masterVolumeParamName);
        _sfxVolume = PlayerPrefs.GetFloat(effectsVolumeParamName);
        _musicVolume = PlayerPrefs.GetFloat(musicVolumeParamName);
        _menusVolume = PlayerPrefs.GetFloat(menusVolumeParamName);
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            SetGroupVolume(masterVolumeParamName, _masterVolume);
            SetGroupVolume(effectsVolumeParamName, _sfxVolume);
            SetGroupVolume(musicVolumeParamName, _musicVolume);
            SetGroupVolume(menusVolumeParamName, _menusVolume);
        }
    }

    private void OnEnable()
    {
        VolumeMenu.onMasterVolumeChanged += SetMasterVolume;
        VolumeMenu.onEffectsVolumeChanged += SetEffectsVolume;
        VolumeMenu.onMusicVolumeChanged += SetMusicVolume;
        VolumeMenu.onMenusVolumeChanged += SetMenusVolume;

        ColorFilterSwitch.onSwapColor += PlaySwapColorSound;
        ColorCollectable.onColorCollected += PlayLockedColorSound;
        LevelManager.onLevelEnd += PlayEndLevelSound;
    }

    private void OnDisable()
    {
        VolumeMenu.onMasterVolumeChanged -= SetMasterVolume;
        VolumeMenu.onEffectsVolumeChanged -= SetEffectsVolume;
        VolumeMenu.onMusicVolumeChanged -= SetMusicVolume;
        VolumeMenu.onMenusVolumeChanged -= SetMenusVolume;

        ColorFilterSwitch.onSwapColor -= PlaySwapColorSound;
        ColorCollectable.onColorCollected -= PlayLockedColorSound;
        LevelManager.onLevelEnd -= PlayEndLevelSound;
    }

    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
//        if (!volumeSet)
//#if UNITY_EDITOR
//            Debug.LogError("The AudioMixer parameter was not found");
//#endif
    }


    private float NormalizedToMixerValue(float normalizedValue)
    {
        //return (normalizedValue - 1f) * 80f;
        return Mathf.Log10(normalizedValue) * 20;
    }

    void SetMasterVolume(float value)
    {
        Debug.Log(value);
        _masterVolume = value;
        PlayerPrefs.SetFloat(masterVolumeParamName, _masterVolume);
        SetGroupVolume(masterVolumeParamName, _masterVolume);
    }

    void SetEffectsVolume(float value)
    {
        _sfxVolume = value;
        PlayerPrefs.SetFloat(effectsVolumeParamName, _sfxVolume);
        SetGroupVolume(effectsVolumeParamName, _sfxVolume);
    }

    void SetMusicVolume(float value)
    {
        _musicVolume = value;
        PlayerPrefs.SetFloat(musicVolumeParamName, _musicVolume);
        SetGroupVolume(musicVolumeParamName, _musicVolume);
    }

    void SetMenusVolume(float value)
    {
        _menusVolume = value;
        PlayerPrefs.SetFloat(menusVolumeParamName, _menusVolume);
        SetGroupVolume(menusVolumeParamName, _menusVolume);
    }



    void PlaySwapColorSound()
    {
        PlaySound(defaultAudioSource, soundBank.swapColorSound);
    }

    void PlayLockedColorSound(Filter filter, Vector3 position)
    {
        PlaySound(defaultAudioSource, soundBank.lockedColorSound);
    }

    void PlayTranslationSound()
    {
        if (defaultAudioSource.isPlaying == false)
        {
            PlaySound(defaultAudioSource, soundBank.translationSound);
        }        
    }

    void PlayEndLevelSound()
    {
        PlaySound(defaultAudioSource, soundBank.endlevelSound);
    }

    void PlaySound(AudioSource source, SoundBank.Sound sound)
    {
        source.clip = sound.clip;
        source.outputAudioMixerGroup = sound.mixerGroup;
        source.Play();
    }

    public void StopCurrent()
    {
        defaultAudioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        defaultAudioSource.volume = volume;
    }
}
