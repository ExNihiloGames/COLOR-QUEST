using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public LayerMask walkableMask;
    public float speed;
    public float tolerance;

    private Vector2 moveInput;
    private Grid grid;
    private GameObject player;
    //private Player playerScript;
    private PlayerInputs playerInputs;
    private Node playerNode;
    private Animator animator;
    private bool isMoving;

    public Vector3 LastCheckpoint => lastCheckpoint;
    private Vector3 lastCheckpoint;

    public static Action onPlayerReturnToCheckpoint;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        player = GameObject.FindWithTag("Player");
        animator = player.GetComponent<Animator>();
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        EnablePlayerInputs(true);
        GameManager.onPlayerDeath += ResetOnDeath;
        GameManager.onPause += DisablePlayerInputs;
        LevelManager.onEagleView += DisablePlayerInputs;
        ColorCollectable.onColorCollected += OnColorCollected;
    }

    private void OnDisable()
    {
        EnablePlayerInputs(false);
        GameManager.onPlayerDeath -= ResetOnDeath;
        GameManager.onPause -= DisablePlayerInputs;
        LevelManager.onEagleView -= DisablePlayerInputs;
        ColorCollectable.onColorCollected -= OnColorCollected;
    }

    public void EnablePlayerInputs(bool enable)
    {
        if (enable)
        {
            playerInputs.PlayerMovement.Enable();
        }
        else
        {
            playerInputs.PlayerMovement.Disable();
        }
    }

    public void DisablePlayerInputs(bool disabled)
    {
        if (disabled)
        {
            playerInputs.PlayerMovement.Disable();
        }
        else
        {
            playerInputs.PlayerMovement.Enable();
        }
    }

    void Start()
    {
        lastCheckpoint = player.transform.position;
        playerNode = grid.NodeFromWorldPoint(player.transform.position);
    }

    private void Update()
    {
        moveInput = playerInputs.PlayerMovement.Move.ReadValue<Vector2>();
        float moveHorizontal = moveInput.x;
        float moveVertical = moveInput.y;

        if (moveHorizontal != 0 && isMoving)
        {
            animator.SetBool("moveHorizontal", true);
            animator.SetBool("moveVertical", false);
        }
        else
        {
            animator.SetBool("moveHorizontal", false);
        }
        if (moveVertical != 0 && isMoving)
        {
            animator.SetBool("moveVertical", true);
            animator.SetBool("moveHorizontal", false);
        }
        else
        {
            animator.SetBool("moveVertical", false);
        }

        if (moveHorizontal < 0 && !isMoving)
        {
            TryMovePlayer((int)playerNode.gridPos.x - 1, (int)playerNode.gridPos.y);
        }
        else if (moveHorizontal > 0 && !isMoving)
        {
            TryMovePlayer((int)playerNode.gridPos.x + 1, (int)playerNode.gridPos.y);
        }
        else if (moveVertical < 0 && !isMoving)
        {
            TryMovePlayer((int)playerNode.gridPos.x, (int)playerNode.gridPos.y - 1);
        }
        else if (moveVertical > 0 && !isMoving)
        {
            TryMovePlayer((int)playerNode.gridPos.x, (int)playerNode.gridPos.y + 1);
        }
    }

    private void TryMovePlayer(int gridPosX, int gridPosY)
    {
        grid.BakeAroundNode(playerNode);
        Node newNode = grid.GetNode(gridPosX, gridPosY);

        if (newNode != null && player.GetComponent<Player>().locked == false)
        {
            if (newNode.walkable && playerNode.walkable)
            {
                Vector3 tmp = grid.WorldPointfromGridPosition(newNode.gridPos);
                tmp = new Vector3(tmp.x, player.transform.position.y, tmp.z);
                if (Physics.CheckSphere(tmp, 0.25f, walkableMask) && !Physics.CheckSphere(new Vector3(tmp.x, tmp.y + 0.5f, tmp.z), 0.25f, walkableMask))
                {
                    StartCoroutine(MovePlayer(newNode));
                }
                else
                {
                    Debug.Log("Can't move: Obstacle in the way");
                    Debug.Log("Is there ground ? " + Physics.CheckSphere(tmp, 0.25f, walkableMask) + "Is there Obstacle? " + Physics.CheckSphere(new Vector3(tmp.x, tmp.y + 0.5f, tmp.z), 0.25f, walkableMask));
                }
            }
            else
            {
                Debug.Log("Can't move: Node is not walkable or player node is not walkable");
            }
        }
        else
        {
            Debug.Log("Can't move: Node is Null or player is LOCKED");
        }
    }

    public void UpdatePlayerNode()
    {
        playerNode = grid.NodeFromWorldPoint(player.transform.position);
    }

    void ResetOnDeath()
    {
        StartCoroutine(ResetToLastCheckpoint(1));
    }

    public IEnumerator ResetToLastCheckpoint(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        player.transform.position = lastCheckpoint;
        playerNode = grid.NodeFromWorldPoint(player.transform.position);

        onPlayerReturnToCheckpoint?.Invoke();
    }

    private IEnumerator MovePlayer(Node targetNode)
    {
        isMoving = true;
        Vector3 targetDestination = targetNode.worldPos;
        targetDestination.y = player.transform.position.y;
        while (Vector3.Distance(player.transform.position, targetDestination) > tolerance)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetDestination, speed*Time.deltaTime);
            yield return null;
        }
        player.transform.position = targetDestination;
        UpdatePlayerNode();
        isMoving = false;
    }

    private void OnColorCollected(Filter filter, Vector3 position)
    {
        lastCheckpoint = position;
    }
}
