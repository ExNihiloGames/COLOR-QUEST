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

    public static Action onRotateStart;
    public static Action onRotateEnd;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerInputs = new PlayerInputs();
        cam = GetComponent<Camera>();
        distance = Vector3.Distance(transform.position, player.position);
        ogOrthographicSize = cam.orthographicSize;
        transform.LookAt(player.position);
        offset = transform.position - player.position;
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
        GameManager.onPlayerDeath += ShakeCoroutine;
    }

    private void OnDisable()
    {
        EnablePlayerInputs(false);
        GameManager.onPlayerDeath -= ShakeCoroutine;
    }

    public void EnablePlayerInputs(bool enable)
    {
        if (enable)
        {
            playerInputs.EagleViewCamera.Enable();
            playerInputs.EagleViewCamera.Rotate.performed += OnRotate;
        }
        else
        {
            playerInputs.EagleViewCamera.Rotate.performed -= OnRotate;
            playerInputs.EagleViewCamera.Disable();
        }
    }

    public void Teleport()
    {
        transform.position = player.position + offset;
    }

    public void SetEagleView(bool enable)
    {
        if (enable)
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
        float dir = rotateInput < 0 ? 1f : -1f; // Previously -1f : 1f
        StartCoroutine(Rotate(dir));
    }

    IEnumerator EagleView()
    {
        while (cam.orthographicSize < eagleViewSetup.evOrthographicSize)
        {
            cam.orthographicSize += eagleViewSetup.zoomSpeed * Time.deltaTime;
            yield return null;
        }
        cam.orthographicSize = eagleViewSetup.evOrthographicSize;
        EnablePlayerInputs(true);
        _isEagleView = true;
    }

    IEnumerator Rotate(float direction)
    {
        float rot = 0f;
        onRotateStart?.Invoke();
        EnablePlayerInputs(false);
        while (rot < eagleViewSetup.stepRot)
        {
            transform.RotateAround(player.position, Vector3.up, direction * eagleViewSetup.rotationSpeed * Time.deltaTime);
            transform.position = player.position - transform.forward * distance;
            rot += eagleViewSetup.rotationSpeed * Time.deltaTime;
            yield return null;
        }
        cumulativeRot += rot * direction;
        onRotateEnd?.Invoke();
        EnablePlayerInputs(true);
    }

    IEnumerator ResetCamera()
    {
        float direction = 0f;
        float rot = 0f;
        float moduloCumulativeRot = cumulativeRot % 360;

        onRotateStart?.Invoke();
        EnablePlayerInputs(false);
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
        onRotateEnd?.Invoke();
        _isEagleView = false;
    }

    void ShakeCoroutine()
    {
        StartCoroutine(Shake());
    }

    public IEnumerator Shake()
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
