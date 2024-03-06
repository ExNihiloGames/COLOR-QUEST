using UnityEngine;

public class SimplePlatform : MonoBehaviour
{
    private GameObject player;
    public GameObject Player { get { return player; } }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.SetParent(transform, true);
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.SetParent(null);
            player = null;
        }
    }
}
