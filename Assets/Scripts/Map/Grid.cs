using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask walkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public bool drawGizmos = true;
    public bool drawWalkMap = false;

    Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX;
    public int GridSizeX { get { return gridSizeX; } }
    private int gridSizeY;
    public int GridSizeY { get { return gridSizeY; } }
    private Vector3 worldBL;

    // Start is called before the first frame update
    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        worldBL = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        for(int i=0; i<gridSizeX; i++)
        {
            for (int j=0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = WorldPointfromGridPosition(new Vector2(i, j));
                grid[i, j] = new Node(RaycastOnWorlPoint(worldPoint), worldPoint, new Vector2(i, j));
            }
        }
    }

    public Node GetNode(int x, int y) 
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            return grid[x, y];
        }
        return null;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public Vector3 WorldPointfromGridPosition(Vector2 gridPosition)
    {
        return worldBL + Vector3.right * ((int)gridPosition.x * nodeDiameter + nodeRadius) + Vector3.forward * ((int)gridPosition.y * nodeDiameter + nodeRadius);
    }

    public void Bake()
    {
        for (int i=0; i<gridSizeX;i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = WorldPointfromGridPosition(new Vector2(i, j));
                grid[i, j].walkable= RaycastOnWorlPoint(worldPoint);
            }
        }      
    }

    public void BakeAroundNode(Node node)
    {
        int posX = (int)node.gridPos.x;
        int posY = (int)node.gridPos.y;

        for (int i=-1; i<2; i++)
        {
            for (int j=-1; j<2; j++)
            {
                if ((posX + i) < 0 || (posX + i) >= gridSizeX || (posY + j) < 0 || (posY + j) >= gridSizeY) continue;
                Vector3 worldPoint = WorldPointfromGridPosition(new Vector2(posX + i, posY + j));
                grid[posX + i, posY + j].walkable = RaycastOnWorlPoint(worldPoint);
            }
        }
    }

    private bool RaycastOnWorlPoint(Vector3 worldPoint)
    {
        Ray ray = new Ray(worldPoint + Vector3.up * 10, Vector3.down);
        return Physics.Raycast(ray, out _, 30, walkableMask);     
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
            if (grid != null)
            {
                foreach (Node node in grid)
                {
                    if (drawWalkMap)
                    {
                        Gizmos.color = node.walkable ? Color.white : Color.black;
                        Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - (nodeDiameter / 10)));
                    }                    
                }
            }
        }        
    }
}
