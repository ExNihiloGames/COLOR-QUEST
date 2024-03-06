using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public GameObject parentMenu;
    public GameObject optionsMenuUI;
    public GameObject controlsMenuUI;
    public GameObject volumeMenuUI;
    public AudioMixer audioMixer;

    public void ShowOptions()
    {
        parentMenu.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void HideOptions()
    {
        parentMenu.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void ShowControls()
    {
        optionsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(true);
    }
    public void HideControls()
    {
        optionsMenuUI.SetActive(true);
        controlsMenuUI.SetActive(false);
    }

    public void ShowVolume()
    {
        optionsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(true);
    }

    public void HideVolume()
    {
        optionsMenuUI.SetActive(true);
        volumeMenuUI.SetActive(false);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("mainVolume", volume);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
}
