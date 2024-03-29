using System;
using UnityEngine;


public class Player : MonoBehaviour
{
    public GameObject cyanOrbitor;
    public GameObject magentaOrbitor;
    public GameObject yellowOrbitor;
    public Transform groundCheck;
    public LayerMask walkableLayer;

    private BoxCollider boxCollider;
    private ParticleSystem deathEffect;
    private float _gravity = -9.81f;
    private bool _isFalling = false;
    private bool _locked = false;
    public bool locked { get { return _locked; } set { _locked = value; } }    

    public static Action PlayerFalls;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        deathEffect = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!Physics.CheckSphere(groundCheck.position, 0.25f, walkableLayer))
        {
            Vector3 pos = transform.position;

            if (!_isFalling)
            {
                PlayerFalls?.Invoke();
                _isFalling = true;
            }

            transform.position = new Vector3(pos.x, pos.y + _gravity * Time.deltaTime, pos.z);            
        }
    }

    private void OnEnable()
    {
        ColorCollectable.onColorCollected += ActivateOrbitor;
        GameManager.onPlayerDeath += Die;
        GameManager.onPlayerRespawn += Respawn;
    }

    private void OnDisable()
    {
        ColorCollectable.onColorCollected -= ActivateOrbitor;
        GameManager.onPlayerDeath -= Die;
        GameManager.onPlayerRespawn -= Respawn;
    }

    private void OnDestroy()
    {
        ColorCollectable.onColorCollected -= ActivateOrbitor;
        GameManager.onPlayerDeath -= Die;
        GameManager.onPlayerRespawn -= Respawn;
    }

    public void ActivateOrbitor(Filter orbitorColor, Vector3 position)
    {
        switch(orbitorColor)
        {
            case Filter.Cyan:
                cyanOrbitor.SetActive(true); break;
            case Filter.Magenta:
                magentaOrbitor.SetActive(true); break;
            case Filter.Yellow:
                yellowOrbitor.SetActive(true); break;
            default:
                break;
        }
    }

    public void Die()
    {
        _locked = true;
        boxCollider.enabled = false;
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        if (deathEffect.isPlaying == false)
        {
            deathEffect.Play();
        }
    }

    public void Respawn()
    {
        _isFalling = false;
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        boxCollider.enabled = true;
        _locked = false;
    }
}
