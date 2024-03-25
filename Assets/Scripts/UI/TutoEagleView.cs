using UnityEngine;
using UnityEngine.InputSystem;

public class TutoEagleView : MonoBehaviour
{
    public GameObject enterEagleView;
    public GameObject camRotate;
    public GameObject exitEagleView;
    private PlayerInputs playerInputs;
    private bool eagleViewOn;

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.EagleViewCamera.Enable();
        playerInputs.EagleViewCamera.ActivateEagleView.performed += OnEagleViewEnter;
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
