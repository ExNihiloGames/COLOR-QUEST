using System;
using System.Linq;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Range(0f, 10f)]
    public float emissiveIntensity = 3f;
    public Filter colorFilter;
    public float lifetime;
    public GameObject explosionEffect;
    private float force;
    public float Force { set { force = value; } }
    private Renderer _renderer;
    private const string EMISSIVE_COLOR_NAME = "_EmissionColor";
    private const string EMISSIVE_KEYWORLD = "_EMISSION";
    private Rigidbody rb;

    public static Action onPlayerCollision;

    void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        foreach (Material material in _renderer.materials)
        {
            if (_renderer.material.enabledKeywords.Any(item => item.name == EMISSIVE_KEYWORLD) && _renderer.material.HasColor(EMISSIVE_COLOR_NAME))
            {
                material.SetColor(EMISSIVE_COLOR_NAME, _renderer.material.color * emissiveIntensity);
            }
        }
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force);
    }

    private void Update()
    {
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            lifetime -= Time.deltaTime;
        }
    }

    private void DestroyedByContact() 
    {
        Instantiate(explosionEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onPlayerCollision?.Invoke();
            DestroyedByContact();
        }
        else if (other.gameObject.tag == "GameplayObject")
        {
            ColorControl goCO = other.GetComponent<ColorControl>();
            if (goCO != null)
            {
                if (goCO.filter != colorFilter)
                {
                    DestroyedByContact();
                }
            }
        }
    }
}
