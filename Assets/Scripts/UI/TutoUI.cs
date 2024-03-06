using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutoUI : MonoBehaviour
{
    public GameObject slideHolder;
    public GameObject collectableTuto;
    public Sprite switchFilterSprite;
    public Sprite uCanSeeColors;
    public Sprite gatherallColorsTuto;

    private PlayerInputs playerInputs;
    private Image slideImage;
    private bool tutoEnd;

    // Start is called before the first frame update
    void Start()
    {
        playerInputs = new PlayerInputs();
        slideImage = slideHolder.GetComponent<Image>();
        playerInputs.PlayerActions.Enable();
        slideHolder.SetActive(true);
    }

    private void OnEnable()
    {
        GameManager.onPause += DisablePlayerInputs;
    }

    private void OnDisable()
    {
        GameManager.onPause -= DisablePlayerInputs;
    }

    private void Update()
    {
        if (collectableTuto == null && !tutoEnd)
        {
            RectTransform rt = slideHolder.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(400, 400);
            slideImage.sprite = gatherallColorsTuto;
            slideHolder.SetActive(true);
            tutoEnd= true;
        }
    }

    private void TriggerSeeColors(InputAction.CallbackContext context)
    {
        if (!tutoEnd)
        {
            playerInputs.PlayerActions.CycleColorDown.performed -= TriggerSeeColors;
            playerInputs.PlayerActions.CycleColorUp.performed -= TriggerSeeColors;
            playerInputs.PlayerActions.Disable();
            RectTransform rt = slideHolder.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(800, 800);
            slideImage.sprite = uCanSeeColors;
            collectableTuto.SetActive(true);
        }        
    }

    public void ShowHideSprite(Sprite spriteToDisplay=null)
    {
        if (!tutoEnd)
        {
            if (spriteToDisplay != null)
            {
                slideImage.sprite = spriteToDisplay;
                slideHolder.SetActive(true);
                playerInputs.PlayerActions.CycleColorDown.performed += TriggerSeeColors;
                playerInputs.PlayerActions.CycleColorUp.performed += TriggerSeeColors;
            }
            else
            {
                slideHolder.SetActive(false);
            }
        }
    }

    public void DisablePlayerInputs(bool disable)
    {
        if (disable)
        {
            playerInputs.PlayerActions.CycleColorDown.performed -= TriggerSeeColors;
            playerInputs.PlayerActions.CycleColorUp.performed -= TriggerSeeColors;
            playerInputs.PlayerActions.Disable();
        }
        else
        {
            playerInputs.PlayerActions.Enable();
            playerInputs.PlayerActions.CycleColorDown.performed += TriggerSeeColors;
            playerInputs.PlayerActions.CycleColorUp.performed += TriggerSeeColors;
        }
    }

    private void OnDestroy()
    {
        DisablePlayerInputs(true);
    }
}
