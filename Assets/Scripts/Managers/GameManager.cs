using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject levelTransitionCanvas;
    public GameObject crossfade;
    public GameObject loadingScreen;
    public Slider loadingBar;


    [SerializeField] bool isPaused = false;

    public static Action<bool> onPause;
    public static Action onPlayerDeath;
    public static Action onPlayerRespawn;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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
        DontDestroyOnLoad(gameObject);
        if (SceneManager.GetActiveScene().buildIndex == (int)Scene.Manager)
        {
            StartCoroutine(LoadAsynchronously((int)Scene.Titre));
        }
    }

    private void OnEnable()
    {
        PauseMenu.onPauseInput += OnPauseInput;
        MenuBase.onLaunchTutorial += LoadTutorial;
        MenuBase.onMainMenu += LoadTitleScreen;
        MenuBase.onExitGame += ExitGame;
        MenuBase.onCredits += LoadCreditScene;

        ResetFallingPlayer.onPlayerFall += OnPlayerDeath;
        CannonBall.onPlayerCollision += OnPlayerDeath;
        Laser.onLaserHitPlayer += OnPlayerDeath;
        Traps.onPlayerHitTrap += OnPlayerDeath;

        Movement.onPlayerReturnToCheckpoint += OnPlayerRespawn;

        LevelManager.onRequestNextLevel += LoadNextLevel;
        LevelManager.onLevelReset += ResetLevel;
    }

    private void OnDisable()
    {
        PauseMenu.onPauseInput -= OnPauseInput;
        MenuBase.onLaunchTutorial -= LoadTutorial;
        MenuBase.onMainMenu -= LoadTitleScreen;
        MenuBase.onExitGame -= ExitGame;
        MenuBase.onCredits -= LoadCreditScene;

        ResetFallingPlayer.onPlayerFall -= OnPlayerDeath;
        CannonBall.onPlayerCollision -= OnPlayerDeath;
        Laser.onLaserHitPlayer -= OnPlayerDeath;
        Traps.onPlayerHitTrap -= OnPlayerDeath;

        Movement.onPlayerReturnToCheckpoint -= OnPlayerRespawn;

        LevelManager.onRequestNextLevel -= LoadNextLevel;
        LevelManager.onLevelReset -= ResetLevel;
    }

    public enum Scene
    {
        Manager = 0,
        Titre = 1,
        Niv_00 = 2,
        Niv_01 = 3,
        Niv_02 = 4,
        Niv_03 = 5,
        Niv_04 = 6,
        Niv_05 = 7,
        Niv_06 = 8,
        Niv_07 = 9,
        Niv_08 = 10,
        Credit = 11
    }

    private void OnPlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }

    private void OnPlayerRespawn()
    {
        onPlayerRespawn?.Invoke();
    }

    private void OnPauseInput()
    {
        if (isPaused)
        {
            Debug.Log("Resume game");
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            onPause?.Invoke(false);
        }
        else
        {
            Debug.Log("Pause game");
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            onPause?.Invoke(true);
        }
        isPaused = !isPaused;
    }


    public void Crossfade()
    {
        levelTransitionCanvas.SetActive(true);
        LeanTween.alpha(crossfade.GetComponent<RectTransform>(), 1f, 1f);
        LeanTween.alpha(crossfade.GetComponent<RectTransform>(), 0f, 1f).setDelay(1f);
    }

    public void LoadLevel(Scene scene)
    {
        Crossfade();
        StartCoroutine(LoadAsynchronously((int)scene));
        Crossfade();
    }

    public void LoadTitleScreen()
    {
        Time.timeScale = 1f;
        Crossfade();
        StartCoroutine(LoadAsynchronously((int)Scene.Titre));
        Crossfade();
    }

    public void LoadTutorial()
    {
        Crossfade();
        StartCoroutine(LoadAsynchronously((int)Scene.Niv_00));
        Crossfade();
    }

    public void LoadCreditScene()
    {
        Crossfade();
        StartCoroutine(LoadAsynchronously((int)Scene.Credit));
        Crossfade();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel()
    {
        Crossfade();
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
        Crossfade();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {        
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
    }
}
