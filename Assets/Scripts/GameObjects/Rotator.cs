using System;
using System.Linq;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public GameObject[] orbitorObjects;
    public float rotationSpeed;
    [Range(0.1f, 0.25f)]
    public float scaleMultplier;
    [Range(1f, 5f)]
    public float emissiveIntensity;

    private const string EMISSIVE_COLOR_NAME = "_EmissionColor";
    private const string EMISSIVE_KEYWORLD = "_EMISSION";

    private void Start()
    {
        foreach (GameObject orbitor in orbitorObjects)
        {
            Renderer _renderer = orbitor.GetComponent<Renderer>();
            foreach (Material material in _renderer.materials)
            {
                if (_renderer.material.enabledKeywords.Any(item => item.name == EMISSIVE_KEYWORLD) && _renderer.material.HasColor(EMISSIVE_COLOR_NAME))
                {
                    material.SetColor(EMISSIVE_COLOR_NAME, _renderer.material.color * emissiveIntensity);
                }
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rescaling = Vector3.one * (0.5f + scaleMultplier * Mathf.Sin(Time.time));
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + rotationSpeed * Time.deltaTime, transform.localRotation.eulerAngles.z);
        foreach (GameObject orbitor in orbitorObjects)
        {
            orbitor.transform.localScale = rescaling;
        }     
    }
}
