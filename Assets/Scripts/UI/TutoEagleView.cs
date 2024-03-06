using UnityEngine;
using UnityEngine.InputSystem;

public class TutoEagleView : MonoBehaviour
{
    public GameObject enterEagleView;
    public GameObject camRotate;
    public GameObject exitEagleView;
    private PlayerInputs playerInputs;
    private bool eagleViewOn;

    // Start is called before the first frame update
    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.PlayerActions.Enable();
        playerInputs.PlayerActions.EagleViewCam.performed += OnEagleViewEnter;
        playerInputs.EagleViewCamera.Enable();
        playerInputs.EagleViewCamera.Rotate.performed += onCameraRotate;
    }

    private void OnEagleViewEnter(InputAction.CallbackContext context)
    {
        if (!eagleViewOn)
        {
            Destroy(enterEagleView);
            Invoke("ShowRotate", 1f);
        }
        else
        {
            Destroy(exitEagleView);
            Destroy(gameObject);
        }
        eagleViewOn = !eagleViewOn;
    }

    private void ShowRotate()
    {
        camRotate.SetActive(true);
    }
    private void onCameraRotate(InputAction.CallbackContext context)
    {
        Destroy(camRotate);
        exitEagleView.SetActive(true);
    }
}
