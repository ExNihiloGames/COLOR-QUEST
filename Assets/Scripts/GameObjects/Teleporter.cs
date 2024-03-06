using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform outgate;
    public ParticleSystem teleportEffect;

    private GameObject player;
    private Movement playerMovement;
    private MainCamera mainCamera;

    private float baseParticleLifeTime;
    private float baseParticleSpeed;
    private float baseEmissionRate;


    private void Start()
    {
        playerMovement= FindObjectOfType<Movement>();
        mainCamera = FindObjectOfType<MainCamera>();
        baseParticleLifeTime = teleportEffect.main.startLifetime.constant;
        baseParticleSpeed = teleportEffect.main.startSpeed.constant;
        baseEmissionRate = teleportEffect.emission.rateOverTime.constant;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        var main = teleportEffect.main;
        var emission = teleportEffect.emission;

        // Lock movement of the player
        player.GetComponent<Player>().locked= true;
        // Modify this gate particle system to realize the actual teleport effect
        main.startLifetime = 1;
        main.startSpeed = 14;
        emission.rateOverTime = 10;
        // Start the teleport effect of the outgate
        outgate.gameObject.GetComponentInChildren<ParticleSystem>().Play();
        player.GetComponent<Animator>().SetTrigger("teleport");
        // Wait half the teleport animation duration of the player
        yield return new WaitForSeconds(0.5f);
        player.transform.position = new Vector3(outgate.position.x, outgate.position.y +0.1f, outgate.position.z);
        playerMovement.UpdatePlayerNode();
        mainCamera.Teleport();
        // Wait the other half teleport animation duration of the player
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Player>().locked = false;
        player = null;
        // Reset this gate particle system to the idle effect
        main.startLifetime = baseParticleLifeTime;
        main.startSpeed = baseParticleSpeed;
        emission.rateOverTime = baseEmissionRate;
    }
}
