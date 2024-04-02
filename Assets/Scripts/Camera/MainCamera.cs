using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class EagleViewSetup
{
    [Range(10f, 20f)]
    public float evOrthographicSize = 20;
    [Range(10f, 50f)]
    public float zoomSpeed = 30f;
    [Range(10f, 90f)]
    public float stepRot = 90f;
    [Range(10f, 50f)]
    public float rotationSpeed = 50f;
}

[System.Serializable]
public class CameraShakeSetup
{
    [Range(0f, 1f)]
    public float shakeDuration = 0.2f;
    [Range(0f, 1f)]
    public float shakeMagnitude = 0.5f;
}

public class MainCamera : MonoBehaviour
{
    public EagleViewSetup eagleViewSetup;
    public CameraShakeSetup cameraShakeSetup;
    [Range(1f, 5f)]
    public float smoothing = 2.5f;

    private Camera cam;
    private PlayerInputs playerInputs;
    private Transform player;
    private Vector3 offset;
    private float distance;
    private float ogOrthographicSize;
    private float cumulativeRot;
    private bool _isEagleView;
    public bool isEagleView { get { return _isEagleView; } }

    public static Action<bool> EagleViewStateChange;
    public static Action onRotateStart;
    public static Action onRotateEnd;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerInputs = new PlayerInputs();
        cam = GetComponent<Camera>();
        distance = Vector3.Distance(transform.position, player.position);
        ogOrthographicSize = cam.orthographicSize;
        offset = transform.position - player.position;

        transform.LookAt(player.position);

        playerInputs.EagleViewCamera.Enable();
        EnableEagleViewActivation(true);
        EnableEagleViewRotation(false);
    }

    void LateUpdate()
    {        
        if (!_isEagleView)
        {
            Vector3 targetCamPos = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        if (playerInputs !=null)
        {
            playerInputs.EagleViewCamera.Enable();
        }
        GameManager.onPlayerDeath += ShakeCoroutine;
    }

    private void OnDisable()
    {
        if (playerInputs != null)
        {
            playerInputs.EagleViewCamera.Enable();
        }
        GameManager.onPlayerDeath -= ShakeCoroutine;
    }

    private void OnDestroy()
    {
        EnableEagleViewActivation(false);
        EnableEagleViewRotation(false);
        playerInputs.EagleViewCamera.Disable();
        GameManager.onPlayerDeath -= ShakeCoroutine;
    }

    public void EnableEagleViewActivation(bool enable)
    {
        if (enable)
        {
            playerInputs.EagleViewCamera.ActivateEagleView.performed += OnEagleView;
            return;
        }
        playerInputs.EagleViewCamera.ActivateEagleView.performed -= OnEagleView;
    }

    public void EnableEagleViewRotation(bool enable)
    {
        if (enable)
        {
            playerInputs.EagleViewCamera.Rotate.performed += OnRotate;
            return;
        }
        playerInputs.EagleViewCamera.Rotate.performed -= OnRotate;
    }

    public void Teleport()
    {
        transform.position = player.position + offset;
    }

    private void OnEagleView(InputAction.CallbackContext context)
    {
        if (!_isEagleView)
        {
            StartCoroutine(EagleView());
        }
        else
        {
            StartCoroutine(ResetCamera());
        }
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        float rotateInput = playerInputs.EagleViewCamera.Rotate.ReadValue<float>();
        float dir = rotateInput < 0 ? 1f : -1f;
        StartCoroutine(Rotate(dir));
    }

    IEnumerator EagleView()
    {
        EnableEagleViewActivation(false);
        EnableEagleViewRotation(false);

        while (cam.orthographicSize < eagleViewSetup.evOrthographicSize)
        {
            cam.orthographicSize += eagleViewSetup.zoomSpeed * Time.deltaTime;
            yield return null;
        }
        cam.orthographicSize = eagleViewSetup.evOrthographicSize;
        _isEagleView = true;
        EagleViewStateChange?.Invoke(_isEagleView);

        EnableEagleViewActivation(true);
        EnableEagleViewRotation(true);
    }

    IEnumerator Rotate(float direction)
    {
        EnableEagleViewActivation(false);
        EnableEagleViewRotation(false);

        float rot = 0f;
        onRotateStart?.Invoke(); // Event for Tuto
        while (rot < eagleViewSetup.stepRot)
        {
            transform.RotateAround(player.position, Vector3.up, direction * eagleViewSetup.rotationSpeed * Time.deltaTime);
            transform.position = player.position - transform.forward * distance;
            rot += eagleViewSetup.rotationSpeed * Time.deltaTime;
            yield return null;
        }
        cumulativeRot += rot * direction;
        onRotateEnd?.Invoke(); // Event for Tuto

        EnableEagleViewActivation(true);
        EnableEagleViewRotation(true);
    }

    IEnumerator ResetCamera()
    {
        EnableEagleViewActivation(false);
        EnableEagleViewRotation(false);

        float direction = 0f;
        float rot = 0f;
        float moduloCumulativeRot = cumulativeRot % 360;

        onRotateStart?.Invoke(); // Tuto
        // Determines the direction in which to rotate the camera, don't ask too much questions nor touch anything really
        if (moduloCumulativeRot >= 0f && moduloCumulativeRot <= 180f || moduloCumulativeRot >= -360 && moduloCumulativeRot <= -180)
        {
            direction = -1f;
        }
        else if(moduloCumulativeRot > 180f && moduloCumulativeRot < 360f || moduloCumulativeRot > -180f && moduloCumulativeRot < 0f)
        {
            direction = 1f;
        }
        // Specific case handle manually
        if (Mathf.Abs(moduloCumulativeRot) > 265 && moduloCumulativeRot < 275)
        {
            moduloCumulativeRot = 90f;
        }
        while (rot < Mathf.Abs(moduloCumulativeRot))
        {
            transform.RotateAround(player.position, Vector3.up, direction * eagleViewSetup.rotationSpeed * Time.deltaTime);
            transform.position = player.position - transform.forward * distance;
            rot += eagleViewSetup.rotationSpeed * Time.deltaTime;
            yield return null;
        }
        cumulativeRot = 0f;

        while (cam.orthographicSize > ogOrthographicSize)
        {
            cam.orthographicSize -= eagleViewSetup.zoomSpeed * Time.deltaTime;
            yield return null;
        }
        cam.orthographicSize = ogOrthographicSize;
        onRotateEnd?.Invoke(); // Tuto
        _isEagleView = false;
        EagleViewStateChange?.Invoke(_isEagleView);

        EnableEagleViewActivation(true);
    }

    private void ShakeCoroutine()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Vector3 originPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < cameraShakeSetup.shakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * cameraShakeSetup.shakeMagnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * cameraShakeSetup.shakeMagnitude;

            transform.position = new Vector3(originPos.x + x, originPos.y + y, originPos.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originPos;
    }
}
