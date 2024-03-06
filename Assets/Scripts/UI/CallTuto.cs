using UnityEngine;

public class CallTuto : MonoBehaviour
{
    public GameObject elemToDestroy;
    public GameObject spriteToDisplay;

    private void OnTriggerEnter(Collider other)
    {
        if (elemToDestroy != null)
        {
            Destroy(elemToDestroy);
        }
        if (spriteToDisplay != null)
        {
            spriteToDisplay.SetActive(true);
        }
        Destroy(gameObject);
    }
}
