using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeIndicator
{
    public GameObject[] timeIndicators = new GameObject[5];
}


public class Emitter : MonoBehaviour
{
    public TimeIndicator timeIndicator;
    public LayerMask rayLayerMask;
    public ParticleSystem rayParticleEffect;
    public Transform rayOrigin;
    [Range(1f, 5f)]
    public float rayDuration = 5f;
    [Range(1f, 5f)]
    public float rayCoolDown = 3f;
    [Range(1f, 10f)]
    public float rayLength = 5f;

    private List<GameObject> activatedObjects;
    private int indicatorIndex;
    private float nextRay;
    private float rayTimeLeft;
    private bool rayIsOn;
    private float cumulatedDecounter;

    // Start is called before the first frame update
    void Start()
    {
        activatedObjects = new List<GameObject>();
        nextRay = 0f;
        rayTimeLeft = rayDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (rayIsOn)
        {
            rayTimeLeft -= Time.deltaTime;
            cumulatedDecounter += Time.deltaTime;
            if (cumulatedDecounter >= 1f)
            {
                timeIndicator.timeIndicators[indicatorIndex].SetActive(false);
                indicatorIndex += 1;
                cumulatedDecounter = 0f;
            }
            if (rayTimeLeft <= 0f)
            {
                rayParticleEffect.Stop();
                foreach (GameObject obj in activatedObjects)
                {
                    // Deactivate every object in activated objects list
                }
                // Clear activated objects list
                rayIsOn = false;
                nextRay = rayCoolDown;
                rayTimeLeft = rayDuration;
                cumulatedDecounter = 0f;
                indicatorIndex = 0;
            }
        }
        else
        {
            nextRay -= Time.deltaTime;
        }
    }

    public void CastRay()
    {
        if (nextRay <= 0f) 
        {
            rayParticleEffect.Play();
            foreach(GameObject indicator in timeIndicator.timeIndicators)
            {
                indicator.SetActive(true);
            }
            Ray ray = new Ray(new Vector3(rayOrigin.transform.position.x, rayOrigin.transform.position.y, rayOrigin.transform.position.z), rayOrigin.forward);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, rayLength, rayLayerMask);
            if (raycastHits.Length > 0)
            {
                foreach(RaycastHit hit in raycastHits)
                {
                    // Activate hidden Object
                    // Add activated object to activated objects list
                }
            }
        }
    }
}
