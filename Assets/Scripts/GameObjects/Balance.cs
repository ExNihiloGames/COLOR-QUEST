using UnityEngine;

public class Balance : MonoBehaviour
{
    public GameObject weightLeft;
    public GameObject platformLeft;

    public GameObject weightRight;
    public GameObject platformRight;

    public float minHeight;
    public float meanHeight;
    public float maxHeight;    
    public float speed;

    private Vector3 platformLeftInitPos;
    private Vector3 platformLeftTargetPos;
    private Vector3 platformRightInitPos;
    private Vector3 platformRightTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        platformLeftInitPos = platformLeft.transform.localPosition;
        platformRightInitPos = platformRight.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if ((weightLeft.activeSelf && weightRight.activeSelf) || (!weightLeft.activeSelf && !weightRight.activeSelf))
        {
            platformLeftTargetPos = new Vector3(platformLeftInitPos.x, meanHeight, platformLeftInitPos.z);
            platformRightTargetPos = new Vector3(platformRightInitPos.x, meanHeight, platformRightInitPos.z);
        }
        else if (weightLeft.activeSelf && !weightRight.activeSelf)
        {
            platformLeftTargetPos = new Vector3(platformLeftInitPos.x, minHeight, platformLeftInitPos.z);
            platformRightTargetPos = new Vector3(platformRightInitPos.x, maxHeight, platformRightInitPos.z);
        }
        else if(!weightLeft.activeSelf && weightRight.activeSelf)
        {
            platformLeftTargetPos = new Vector3(platformLeftInitPos.x, maxHeight, platformLeftInitPos.z);
            platformRightTargetPos = new Vector3(platformRightInitPos.x, minHeight, platformRightInitPos.z);
        }

        if (platformLeft.transform.localPosition != platformLeftTargetPos || platformRight.transform.localPosition != platformRightTargetPos)
        {
            if (platformLeft.GetComponent<SimplePlatform>().Player != null)
            {
                platformLeft.GetComponent<SimplePlatform>().Player.GetComponent<Player>().locked = true;
            }
            else if (platformRight.GetComponent<SimplePlatform>().Player != null)
            {
                platformRight.GetComponent<SimplePlatform>().Player.GetComponent<Player>().locked = true;
            }
            platformLeft.transform.localPosition = Vector3.MoveTowards(platformLeft.transform.localPosition, platformLeftTargetPos, speed * Time.deltaTime);
            platformRight.transform.localPosition = Vector3.MoveTowards(platformRight.transform.localPosition, platformRightTargetPos, speed * Time.deltaTime);
        }
        else
        {
            if (platformLeft.GetComponent<SimplePlatform>().Player != null)
            {
                platformLeft.GetComponent<SimplePlatform>().Player.GetComponent<Player>().locked = false;
            }
            else if (platformRight.GetComponent<SimplePlatform>().Player != null)
            {
                platformRight.GetComponent<SimplePlatform>().Player.GetComponent<Player>().locked = false;
            }
        }
    }
}
