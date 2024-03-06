using System;
using UnityEngine;


[System.Serializable]
public class LaserEffects
{
    public LineRenderer lineRenderer;
    public ParticleSystem chargeUpLaserEffect;
    public ParticleSystem endLaserEffect;
    public ParticleSystem endLaserLight;
}


[System.Serializable]
public class LaserSetup
{
    public LayerMask laserMask;
    public Transform laserOrigin;
    [Range(5, 20)]
    public int laserRange;
}


public class Laser : MonoBehaviour
{
    public LaserEffects laserEffects;
    public LaserSetup laserSetup;
    private float playerhit;
    private float playerHitRest = 2f;

    public static Action onLaserHitPlayer;
    // Start is called before the first frame update
    void Start()
    {
        laserEffects.lineRenderer.SetPosition(1, new Vector3(0f, 0f, laserSetup.laserRange));
        laserEffects.lineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (laserEffects.chargeUpLaserEffect.isPlaying == false)
        {
            laserEffects.chargeUpLaserEffect.Play();
        }

        Vector3 laserOG = laserSetup.laserOrigin.transform.position;
        Vector3 laserDir = laserSetup.laserOrigin.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(laserOG, laserDir, out hit, laserSetup.laserRange, laserSetup.laserMask))
        {
            float distance = Vector3.Distance(hit.point, laserOG);
            laserEffects.lineRenderer.SetPosition(1, new Vector3(0f, 0f, distance * 1.34f));
            laserEffects.endLaserEffect.transform.position = hit.point;
            // laserEffects.endLaserLight.transform.position = hit.point;
            // laserEffects.endLaserLight.enabled = true;
            if (!laserEffects.endLaserEffect.isPlaying)
            {
                laserEffects.endLaserEffect.Play();
            }
            if (hit.collider.gameObject.tag == "Player")
            {
                if (playerhit <= 0f)
                {
                    onLaserHitPlayer?.Invoke();
                    playerhit = playerHitRest;
                }                
            }
            else
            {
                playerhit -= Time.deltaTime;
            }
        }
        else
        {
            laserEffects.lineRenderer.SetPosition(1, new Vector3(0f, 0f, laserSetup.laserRange));
            if (laserEffects.endLaserEffect.isPlaying)
            {
                laserEffects.endLaserEffect.Stop();
                // laserEffects.endLaserLight.enabled = false;
            }
            playerhit -= Time.deltaTime;
        }
    }
}
