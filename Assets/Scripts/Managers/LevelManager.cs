using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    //private GameManager gameManager;
    //private SoundManager soundManager;
    private MainCamera mainCamera;
    private PlayerInputs playerInputs;
    //private Movement playerMovement;
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
        GameManager.onPause += DisablePlayerInputs;
    }

    private void OnDisable()
    {
        ColorCollectable.onColorCollected -= OnColorPicked;
        GameManager.onPause -= DisablePlayerInputs;
    }

    // Start is called before the first frame update
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
        if (mainCamera.isEagleView)
        {
            mainCamera.SetEagleView(false);
            onEagleView.Invoke(false);
            //playerMovement.EnablePlayerInputs(true);
        }
        else
        {
            mainCamera.SetEagleView(true);
            onEagleView.Invoke(true);
            //playerMovement.EnablePlayerInputs(false);
        }
    }

    private void LM_ResetLevel(InputAction.CallbackContext context)
    {
        onLevelReset?.Invoke();
    }

    public void DisablePlayerInputs(bool disable)
    {
        if (disable)
        {
            playerInputs.PlayerActions.Reset.performed -= LM_ResetLevel;
            playerInputs.PlayerActions.EagleViewCam.performed -= ActivateEagleView;
            playerInputs.PlayerActions.Disable();            
        }
        else
        {
            playerInputs.PlayerActions.Enable();
            playerInputs.PlayerActions.Reset.performed += LM_ResetLevel;
            playerInputs.PlayerActions.EagleViewCam.performed += ActivateEagleView;
        }
    }

    IEnumerator GoNextLevel()
    {
        DisablePlayerInputs(false);
        yield return new WaitForSeconds(nextLevelTransitionDuration);
        onRequestNextLevel?.Invoke();
    }
}
