using System;
using System.Linq;
using UnityEngine;

public class ColorCollectable : MonoBehaviour
{
    public Filter locksColor = Filter.Cyan;
    [Range(1f, 5f)]
    public float emissiveIntensity;

    private Vector3 startPos;

    public static Action<Filter, Vector3> onColorCollected;

    private const string EMISSIVE_COLOR_NAME = "_EmissionColor";
    private const string EMISSIVE_KEYWORLD = "_EMISSION";

    private void Start()
    {
        startPos = transform.position;
        Renderer _renderer= GetComponentInChildren<Renderer>();
        foreach (Material material in _renderer.materials)
        {
            if (_renderer.material.enabledKeywords.Any(item => item.name == EMISSIVE_KEYWORLD) && _renderer.material.HasColor(EMISSIVE_COLOR_NAME))
            {
                material.SetColor(EMISSIVE_COLOR_NAME, _renderer.material.color * emissiveIntensity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 hoveringMotion = Vector3.up * Mathf.Sin(Time.time);
        transform.position = new Vector3(startPos.x + hoveringMotion.x, startPos.y + hoveringMotion.y, startPos.z + hoveringMotion.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.tag == "Player")
            {
                onColorCollected?.Invoke(locksColor, startPos);
                Destroy(gameObject);
            }
        }
    }
}
