using System;
using System.Linq;
using UnityEngine;

public class ColorControl : MonoBehaviour
{
    public Filter filter = Filter.Cyan;
    [Range(0f, 10f)]
    public float baseEmissiveIntensity=1f;
    [Range(0f, 10f)]
    public float maxEmissiveIntensity=3f;
    public bool isAlwaysVisible;

    private Renderer _renderer;
    private ParticleSystem lockedEffect;
    private const string EMISSIVE_COLOR_NAME = "_EmissionColor";
    private const string EMISSIVE_KEYWORLD = "_EMISSION";
    private const string BASE_EMISSIVE_INTENSITY = "BASE";
    private const string MAX_EMISSIVE_INTENSITY = "MAX";
    private bool isVisible;
    private bool isLocked;


    private void SetEmissive(string _mode)
    {
        _renderer = GetComponent<Renderer>();
        foreach (Material material in _renderer.materials)
        {
            if (_renderer.material.enabledKeywords.Any(item => item.name == EMISSIVE_KEYWORLD) && _renderer.material.HasColor(EMISSIVE_COLOR_NAME))
            {
                if (_mode == BASE_EMISSIVE_INTENSITY)
                {
                    material.SetColor(EMISSIVE_COLOR_NAME, _renderer.material.color * baseEmissiveIntensity);
                }
                else if (_mode== MAX_EMISSIVE_INTENSITY)
                {
                    material.SetColor(EMISSIVE_COLOR_NAME, _renderer.material.color * maxEmissiveIntensity);
                }
            }
        }
    }
    public void SetVisible(bool visible)
    {
        if(!isAlwaysVisible && !isLocked)
        {
            if (visible)
            {
                gameObject.SetActive(true);
                SetEmissive(BASE_EMISSIVE_INTENSITY);
                isVisible = true;
            }
            else
            {                
                gameObject.SetActive(false);
                isVisible = false;
            }
        }        
    }

    public void SetLocked(bool locked)
    {
        if (locked)
        {
            gameObject.SetActive(true);
            SetEmissive(MAX_EMISSIVE_INTENSITY);
            lockedEffect = GetComponent<ParticleSystem>();
            if ( lockedEffect != null)
            {
                lockedEffect.Play();
            }
            else
            {
                lockedEffect = GetComponentInChildren<ParticleSystem>();
                if (lockedEffect != null)
                {
                    lockedEffect.Play();
                }
            }
            isLocked = true;
        }
        else
        {
            isLocked = false;
            if (lockedEffect != null)
            {
                lockedEffect.Stop();
            }            
            SetVisible(isVisible);
        }
    }
}
