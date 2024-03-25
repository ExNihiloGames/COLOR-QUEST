using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
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
        GameManager.onPause += OnGamePaused;
        MainCamera.EagleViewStateChange += OnEagleViewStateChange;
        ColorCollectable.onColorCollected += OnColorPicked;
    }

    private void OnDisable()
    {
        GameManager.onPause -= OnGamePaused;
        MainCamera.EagleViewStateChange -= OnEagleViewStateChange;
        ColorCollectable.onColorCollected -= OnColorPicked;
    }

    private void OnDestroy()
    {        
        GameManager.onPause -= OnGamePaused;
        MainCamera.EagleViewStateChange -= OnEagleViewStateChange;
        ColorCollectable.onColorCollected -= OnColorPicked;
        playerInputs.PlayerActions.Reset.performed -= LM_ResetLevel;
        playerInputs.PlayerActions.Disable();
    }

    void Start()
    {
        MainCamera.EagleViewStateChange += OnEagleViewStateChange;
        playerInputs = new PlayerInputs();
        playerInputs.PlayerActions.Enable();
        playerInputs.PlayerActions.Reset.performed += LM_ResetLevel;

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

    private void OnEagleViewStateChange(bool state)
    {
        onEagleView?.Invoke(state);
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
        }
        else
        {
            playerInputs.PlayerActions.Reset.performed -= LM_ResetLevel;
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
