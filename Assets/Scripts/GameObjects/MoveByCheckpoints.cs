using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByCheckpoints : MonoBehaviour
{
    [Range(1f, 5f)]
    public float speed=3f;

    private MainCamera mainCamera;
    private Vector3 originPos;
    private List<Vector3> points;
    private float tolerance = 0.1f;

    void Start()
    {
        mainCamera = FindObjectOfType<MainCamera>();
        originPos= transform.localPosition;
        points = new List<Vector3>();
    }

    void Update()
    {
        if(points.Count > 0)
        {
            if (Vector3.Distance(transform.localPosition, points[0]) > tolerance)
            {
                if (!mainCamera.isObjectOnScreen(transform) && mainCamera.FocusTarget != transform)
                {
                    mainCamera.FocusOn(transform);
                }
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, points[0], speed * Time.deltaTime);
            }
            else
            {
                if (mainCamera.FocusTarget == transform) { mainCamera.FocusOff(); };
                transform.localPosition = points[0];
                RemoveFirst();
            }
        }
    }

    public void AddCheckpoint(Vector3 pointLocal)
    {
        points.Add(pointLocal);
    }

    public void ResetToOrigin()
    {
        transform.localPosition = originPos;
    }

    private void RemoveFirst()
    {
        points.RemoveAt(0);
    }
}
