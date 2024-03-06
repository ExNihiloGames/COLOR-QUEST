using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public GameObject waitUI;

    private void Start()
    {
        MainCamera.onRotateStart += ShowWaitUI;
        MainCamera.onRotateEnd += HideWaitUI;
    }
    private void OnEnable()
    {
        MainCamera.onRotateStart += ShowWaitUI;
        MainCamera.onRotateEnd += HideWaitUI;
    }
    private void OnDisable()
    {
        MainCamera.onRotateStart -= ShowWaitUI;
        MainCamera.onRotateEnd -= HideWaitUI;
    }

    private void OnDestroy()
    {
        MainCamera.onRotateStart -= ShowWaitUI;
        MainCamera.onRotateEnd -= HideWaitUI;
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
