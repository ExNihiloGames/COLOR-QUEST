using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPos;
    public Vector2 gridPos;

    public Node(bool _walkable, Vector3 _worldPos, Vector2 _gridPos)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridPos = _gridPos;
    }
}
