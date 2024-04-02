using System;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public Filter filterColor = Filter.Cyan;
    public GameObject moveableObject;
    public Vector3 targetPos;

    private MoveByCheckpoints moveableObjCheckpoints;
    private ParticleSystem pressedEffect;
    private Vector3 initPos;
    private bool isVisible;
    private bool isPressed;
    private bool isLocked;
    

    private void Start()
    {
        initPos= transform.localPosition;
        moveableObjCheckpoints = moveableObject.GetComponent<MoveByCheckpoints>();
    }

    public void SetVisible(bool visible)
    {
        if (!isLocked)
        {
            gameObject.SetActive(visible);
            isVisible = visible;
            try
            {
                GameObject playerInstance = moveableObject.GetComponentInChildren<SimplePlatform>().Player;
                if (!visible && playerInstance != null)
                {
                    playerInstance.transform.SetParent(null);
                    playerInstance.SetActive(true);
                }
            }
            catch (NullReferenceException){}
        }
    }

    public void SetPressed()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.4f, transform.localPosition.z);
        moveableObjCheckpoints.AddCheckpoint(targetPos);
        pressedEffect = GetComponent<ParticleSystem>();
        pressedEffect.Play();
        isPressed = true;
    }

    public void SetLocked(bool locked)
    {
        if (locked) 
        {
            gameObject.SetActive(locked);
            isLocked = locked;
        }
        else
        {
            isLocked = locked;
            SetVisible(isVisible);
        }        
    }

    public void ResetTrigger()
    {
        transform.localPosition = initPos;
        pressedEffect = GetComponent<ParticleSystem>();
        pressedEffect.Stop();
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            if (other.gameObject.tag == "Player")
            {
                SetPressed();
            }
        }        
    }
}
