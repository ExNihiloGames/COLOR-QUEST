using System;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public Filter activatingColor = Filter.Cyan;
    public GameObject mechanism;

    private bool isActivated;
    private bool isLocked;

    public static Action onPlayerHitTrap;

    public void Activate(bool active)
    {
        if (isLocked == false) 
        {
            if (active)
            {
                gameObject.SetActive(true);
                ActivateMechanism(true);
                isActivated = true;
            }
            else
            {
                ActivateMechanism(false);
                isActivated = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void SetLocked(bool locked)
    {
        if (locked)
        {
            gameObject.SetActive(true);
            ActivateMechanism(true);
            isLocked= true;
        }
        else
        {
            isLocked = false;
            Activate(isActivated);            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            onPlayerHitTrap?.Invoke();
        }
    }

    private void ActivateMechanism(bool activation)
    {
        if (mechanism != null)
        {
            mechanism.SetActive(activation);
        }        
    }
}
