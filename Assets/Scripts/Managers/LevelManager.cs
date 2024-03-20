using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    private MainCamera mainCamera;
    private PlayerInputs playerInputs;
    private int totalCollectables;
    private int currentCollectablesCount;
    private float nextLevelTransitionDuration = 3;

    public static Action<bool> onEagleView;
    public static Action onLevelEnd;
    public static Action onLevelReset;
    public static Action onRequestNextLevel;

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
    }

    private void OnEnable()
    {
        ColorCollectable.onColorCollected += OnColorPicked;
        GameManager.onPause += OnGamePaused;
    }

    private void OnDisable()
    {
        ColorCollectable.onColorCollected -= OnColorPicked;
        GameManager.onPause -= OnGamePaused;
    }

    private void OnDestroy()
    {
        ColorCollectable.onColorCollected -= OnColorPicked;
        GameManager.onPause -= OnGamePaused;
    }

    void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
        playerInputs = new PlayerInputs();
        playerInputs.PlayerActions.Enable();
        playerInputs.PlayerActions.Reset.performed += LM_ResetLevel;
        playerInputs.PlayerActions.EagleViewCam.performed += ActivateEagleView;

        foreach(ColorCollectable collectable in FindObjectsOfType<ColorCollectable>())
        {
            totalCollectables += 1;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnColorPicked(Filter colorPicked, Vector3 position)
    {
        currentCollectablesCount += 1; 
        if (currentCollectablesCount >= totalCollectables)
        {
            onLevelEnd?.Invoke();
            StartCoroutine(GoNextLevel());
        }
    }

    private void ActivateEagleView(InputAction.CallbackContext context)
    {
        if(mainCamera.isEagleView)
        {
            onEagleView?.Invoke(false);
        }
        else
        {
            onEagleView?.Invoke(true);
        }
        mainCamera.SetEagleView(!mainCamera.isEagleView);
    }

    private void LM_ResetLevel(InputAction.CallbackContext context)
    {
        onLevelReset?.Invoke();
    }

    private void OnGamePaused(bool pauseState)
    {
        EnablePlayerInputs(!pauseState);
    }

    private void EnablePlayerInputs(bool enable)
    {
        if (enable)
        {
            playerInputs.PlayerActions.Enable();
            playerInputs.PlayerActions.Reset.performed += LM_ResetLevel;
            playerInputs.PlayerActions.EagleViewCam.performed += ActivateEagleView;
        }
        else
        {
            playerInputs.PlayerActions.Reset.performed -= LM_ResetLevel;
            playerInputs.PlayerActions.EagleViewCam.performed -= ActivateEagleView;
            playerInputs.PlayerActions.Disable();
        }
    }

    IEnumerator GoNextLevel()
    {
        EnablePlayerInputs(true);
        yield return new WaitForSeconds(nextLevelTransitionDuration);
        onRequestNextLevel?.Invoke();
    }
}
