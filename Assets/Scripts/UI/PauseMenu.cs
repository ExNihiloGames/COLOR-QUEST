using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using System;

public class PauseMenu : MenuBase
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject controlsMenuUI;
    public GameObject volumeMenuUI;
    public static bool isPaused;

    private PlayerInputs playerInputs;
    private TutoUI tutoUI;
    private Resolution[] resolutions;


    public static Action onPauseInput;

    public void Start()
    {
        playerInputs = new PlayerInputs();
        tutoUI = FindObjectOfType<TutoUI>();
        resolutions = Screen.resolutions;

        playerInputs.PauseMenu.Enable();
        playerInputs.PauseMenu.Pause.performed += OnPauseInput;
    }

    private void OnEnable()
    {
        GameManager.onPause += OnPause;
    }
    private void OnDisable()
    {
        GameManager.onPause -= OnPause;
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        onPauseInput?.Invoke();
    }

    private void OnPause(bool isPaused)
    {
        if (isPaused)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(false);
            controlsMenuUI.SetActive(false);
            volumeMenuUI.SetActive(false);
        }
    }

    public void Resume()
    {
        onPauseInput?.Invoke();
    }

    public void ToMainMenu()
    {
        onMainMenu?.Invoke();
    }

    public void ShowOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void HideOptions()
    {
        pauseMenuUI.SetActive(true);
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

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void ExitGame()
    {
        onExitGame?.Invoke();
    }

    private void EnableplayerInputs(bool enable)
    {
        if (enable == false)
        {            
            playerInputs.PauseMenu.Pause.performed -= OnPauseInput;
            playerInputs.PauseMenu.Disable();
        }
        else
        {
            playerInputs.PauseMenu.Enable();
            playerInputs.PauseMenu.Pause.performed += OnPauseInput;
        }
    }

    private void OnDestroy()
    {
        EnableplayerInputs(false);
    }
}
