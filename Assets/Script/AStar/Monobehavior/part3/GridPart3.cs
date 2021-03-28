using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPart3 : MonoBehaviour
{
    public Transform player;
    public GameObject terrain;
    public LayerMask unwalkableMask;
    Vector2 gridWorldSize;
    Mesh TerrainMesh;
    Bounds TerrainBounds;
    public float nodeRadius;
    NodePart3[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start() 
    {
        TerrainMesh = terrain.GetComponent<MeshFilter>().mesh;
        TerrainBounds = TerrainMesh.bounds;
        gridWorldSize = new Vector2(TerrainBounds.size.x * terrain.transform.localScale.x, TerrainBounds.size.z * terrain.transform.localScale.z);
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter); //grid Height
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter); //grid width
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new NodePart3[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2; //forward = z axis // right = x axis // up = y axis
        Debug.DrawLine(worldBottomLeft, worldBottomLeft + Vector3.up*20, Color.yellow, 20000f);
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new NodePart3(walkable,worldPoint, x, y);
            }
        }
    }

    public List<NodePart3> GetNeighbours(NodePart3 node)
    {
        var neighbours = new List<NodePart3>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) {continue;}

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX,checkY]);
                }
            }
        }
        return neighbours;
    }

    public NodePart3 NodeFromWorldPoint(Vector3 worldPosition) 
    {
        float percentX = worldPosition.x/gridWorldSize.x + 0.5f;
		float percentY = worldPosition.z/gridWorldSize.y + 0.5f;

        percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

		return grid[x,y];
	}

    public List<NodePart3> path;
    void OnDrawGizmos() 
    {
       Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); //y on z is normal
       if(grid != null)
       {
           NodePart3 playerNode = NodeFromWorldPoint(player.position);
           foreach (NodePart3 n in grid)
           {
               Gizmos.color = (n.walkable) ? Color.white : Color.red;
                
               if(playerNode == n)
               {
                   Gizmos.color = Color.cyan;
               }
                
                if(path != null)
                {
                    if(path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                
               Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f)); //Vector3.one = Vector3(1,1,1)
           }
       }
    }
}
