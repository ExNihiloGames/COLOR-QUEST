using UnityEngine;

public class DestroyedByLifetime : MonoBehaviour
{
    public float lifetime;

    void Update()
    {
        if (lifetime <= 0)
        { 
            Destroy(gameObject);
        }
        else
        {
            lifetime -= Time.deltaTime;
        }
    }
}
