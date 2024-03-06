using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByCheckpoints : MonoBehaviour
{
    [Range(1f, 5f)]
    public float speed=3f;

    private float tolerance=0.1f;
    private Vector3 originPos;
    private List<Vector3> points;

    // Start is called before the first frame update
    void Start()
    {
        originPos= transform.localPosition;
        points = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(points.Count > 0)
        {
            if (Vector3.Distance(transform.localPosition, points[0]) > tolerance)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, points[0], speed * Time.deltaTime);
            }
            else
            {
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
