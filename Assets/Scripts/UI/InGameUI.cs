using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public GameObject waitUI;

    private void Start()
    {
        MainCamera.RotateStart += ShowWaitUI;
        MainCamera.RotateEnd += HideWaitUI;
    }
    private void OnEnable()
    {
        MainCamera.RotateStart += ShowWaitUI;
        MainCamera.RotateEnd += HideWaitUI;
    }
    private void OnDisable()
    {
        MainCamera.RotateStart -= ShowWaitUI;
        MainCamera.RotateEnd -= HideWaitUI;
    }

    private void OnDestroy()
    {
        MainCamera.RotateStart -= ShowWaitUI;
        MainCamera.RotateEnd -= HideWaitUI;
    }

    private void ShowWaitUI()
    {
        waitUI.SetActive(true);
    }

    private void HideWaitUI()
    {
        waitUI.SetActive(false);
    }
}
