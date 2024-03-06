using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ColorFilterSwitch : MonoBehaviour
{
    private static ColorFilterSwitch _instance;
    public static ColorFilterSwitch Instance { get { return _instance; } }
    public Filter startFilter = Filter.Cyan;
    public VolumeProfile mVolumeProfile;

    private Vignette mVignette;
    private GameObject[] gameplayObjects;
    private GameObject[] gameplayTraps;
    private GameObject[] gameplayTriggers;
    private PlayerInputs playerInputs;
    private UIColorSwitch uiColorSwitch;
    private int currentColor;

    public static Action onSwapColor;

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
    void Start()
    {
        for (int i = 0; i < mVolumeProfile.components.Count; i++)
        {
            if (mVolumeProfile.components[i].name == "Vignette")
            {
                mVignette = (Vignette)mVolumeProfile.components[i];
            }
        }
        playerInputs = new PlayerInputs();
        uiColorSwitch = GetComponent<UIColorSwitch>();
        gameplayObjects = GameObject.FindGameObjectsWithTag("GameplayObject");
        gameplayTraps = GameObject.FindGameObjectsWithTag("GameplayTrap");
        gameplayTriggers = GameObject.FindGameObjectsWithTag("GameplayTrigger");
        currentColor = (int)startFilter;
        ActivateFilter(currentColor, true);
        EnablePlayerInputs(true);
    }

    void OnColorPicked(Filter colorToLock, Vector3 position)
    {
        foreach (GameObject gamePlayObject in gameplayObjects)
        {
            ColorControl goColorControl = gamePlayObject.GetComponent<ColorControl>();
            Filter goColor = goColorControl.filter;

            if (goColor == colorToLock)
            {
                goColorControl.SetLocked(true);
            }
        }

        foreach (GameObject gameplayTrap in gameplayTraps)
        {
            Traps goTraps = gameplayTrap.GetComponent<Traps>();
            Filter goActivation = goTraps.activatingColor;
            if (goActivation == colorToLock)
            {
                goTraps.SetLocked(true);
            }
        }

        foreach (GameObject gameplayTrigger in gameplayTriggers)
        {
            PlatformTrigger trigger = gameplayTrigger.GetComponent<PlatformTrigger>();
            Filter _color = trigger.filterColor;
            if (_color == colorToLock)
            {
                trigger.SetLocked(true);
            }
        }
    }

    //private void CycleColorsUp(InputAction.CallbackContext context)
    //{
    //    currentColor = currentColor + 1 > 2 ? 0 : currentColor + 1;
    //    ActivateFilter(currentColor);
    //}

    //private void CycleColorsDown(InputAction.CallbackContext context)
    //{
    //    currentColor = currentColor - 1 < 0 ? 2 : currentColor - 1;
    //    ActivateFilter(currentColor);
    //}

    private void Cyan(InputAction.CallbackContext context)
    {
        currentColor = 0;
        ActivateFilter(currentColor);
    }

    private void Magenta(InputAction.CallbackContext context)
    {
        currentColor = 1;
        ActivateFilter(currentColor);
    }

    private void Yellow(InputAction.CallbackContext context)
    {
        currentColor = 2;
        ActivateFilter(currentColor);
    }

    public void ActivateFilter(int selectedFilter, bool isLevelStart=false)
    {
        if (isLevelStart == false)
        {
            onSwapColor?.Invoke();
        }        

        foreach(GameObject gamePlayObject in gameplayObjects)
        {
            ColorControl goColorControl= gamePlayObject.GetComponent<ColorControl>();
            int goColor = (int)goColorControl.filter;

            if (goColor == selectedFilter)
            {
                goColorControl.SetVisible(true);
            }
            else
            {
                goColorControl.SetVisible(false);
            }
        }

        foreach(GameObject gameplayTrap in gameplayTraps)
        {
            Traps goTraps= gameplayTrap.GetComponent<Traps>();
            int goActivation = (int)goTraps.activatingColor;
            if (goActivation == selectedFilter)
            {
                goTraps.Activate(true);
            }
            else
            {
                goTraps.Activate(false);
            }
        }

        foreach(GameObject gameplayTrigger in gameplayTriggers)
        {
            PlatformTrigger trigger = gameplayTrigger.GetComponent<PlatformTrigger>();
            int _color = (int)trigger.filterColor;
            if (_color == selectedFilter)
            {
                trigger.SetVisible(true);
            }
            else
            {
                trigger.SetVisible(false);
            }
        }
        uiColorSwitch.SetIconActive((Filter)selectedFilter);
    }


    public void EnablePlayerInputs(bool enable)
    {
        if (!enable)
        {
            //playerInputs.PlayerActions.CycleColorUp.performed -= CycleColorsUp;
            //playerInputs.PlayerActions.CycleColorDown.performed -= CycleColorsDown;
            playerInputs.PlayerActions.Cyan.performed -= Cyan;
            playerInputs.PlayerActions.Magenta.performed -= Magenta;
            playerInputs.PlayerActions.Yellow.performed -= Yellow;
            playerInputs.PlayerActions.Disable();

        }
        else
        {
            playerInputs.PlayerActions.Enable();
            playerInputs.PlayerActions.Cyan.performed += Cyan;
            playerInputs.PlayerActions.Magenta.performed += Magenta;
            playerInputs.PlayerActions.Yellow.performed += Yellow;
            //playerInputs.PlayerActions.CycleColorUp.performed += CycleColorsUp;
            //playerInputs.PlayerActions.CycleColorDown.performed += CycleColorsDown;
        }
        
    }

    public void DisablePlayerInputs(bool disable)
    {
        if (disable)
        {
            //playerInputs.PlayerActions.CycleColorUp.performed -= CycleColorsUp;
            //playerInputs.PlayerActions.CycleColorDown.performed -= CycleColorsDown;
            playerInputs.PlayerActions.Cyan.performed -= Cyan;
            playerInputs.PlayerActions.Magenta.performed -= Magenta;
            playerInputs.PlayerActions.Yellow.performed -= Yellow;
            playerInputs.PlayerActions.Disable();

        }
        else
        {
            playerInputs.PlayerActions.Enable();
            playerInputs.PlayerActions.Cyan.performed += Cyan;
            playerInputs.PlayerActions.Magenta.performed += Magenta;
            playerInputs.PlayerActions.Yellow.performed += Yellow;
            //playerInputs.PlayerActions.CycleColorUp.performed += CycleColorsUp;
            //playerInputs.PlayerActions.CycleColorDown.performed += CycleColorsDown;
        }

    }

    private void OnDestroy()
    {
        EnablePlayerInputs(false);
    }
}


/*[System.Flags]
public enum Filter
{
    White = 0,
    Cyan = 1,
    Magenta = 2,
    Yellow = 4,
    Red = Yellow & Magenta,
    Blue = Cyan & Magenta,
    Green = Yellow & Cyan,
    Black = ~0
}*/

public enum Filter
{
    Cyan,
    Magenta,
    Yellow,
    White
}