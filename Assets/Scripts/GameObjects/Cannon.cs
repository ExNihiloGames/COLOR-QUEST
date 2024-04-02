using System;
using UnityEngine;

[System.Serializable]
public class CannonBallSetup
{
    public GameObject cannonBall;
    [Range(500f, 2000f)]
    public float cannonBallForce;
}


public class Cannon : MonoBehaviour
{
    public CannonBallSetup cbs;
    public Transform spawnPoint;
    public float fireRate;
    public GameObject shootEffect;
    public GameObject parentCannon;
    private Animator animator;
    private float nextFire;

    void Start()
    {
        animator = parentCannon.GetComponent<Animator>();
        nextFire = 0;
    }

    void Update()
    {
        if (nextFire <= 0f)
        {
            Vector3 shootPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
            Quaternion shootDir = spawnPoint.rotation;
            GameObject l_cannonBall = Instantiate(cbs.cannonBall, shootPos, shootDir);
            l_cannonBall.GetComponent<CannonBall>().Force = cbs.cannonBallForce;
            Instantiate(shootEffect, shootPos, shootDir);
            animator.SetTrigger("Fire");
            nextFire += fireRate;
        }
        else
        {
            nextFire -= Time.deltaTime;
        }
    }
}
