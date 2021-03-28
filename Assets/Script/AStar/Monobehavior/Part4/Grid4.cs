using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Grid4 : MonoBehaviour
{
    public bool displayGridGizmos;
    public Transform player;
    public GameObject terrain;
    public LayerMask unwalkableMask;
    Vector2 gridWorldSize;
    Mesh TerrainMesh;
    Bounds TerrainBounds;
    public float nodeRadius;
    Node4[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake() 
    {
        TerrainMesh = terrain.GetComponent<MeshFilter>().mesh;
        TerrainBounds = TerrainMesh.bounds;
        gridWorldSize = new Vector2(TerrainBounds.size.x * terrain.transform.localScale.x, TerrainBounds.size.z * terrain.transform.localScale.z);
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter); //grid Height
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter); //grid width
        CreateGrid();
    }

    public int MaxSize
    {
        get {return gridSizeX * gridSizeY;}
    }

    void CreateGrid()
    {
        grid = new Node4[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2; //forward = z axis // right = x axis // up = y axis
        Debug.DrawLine(worldBottomLeft, worldBottomLeft + Vector3.up*20, Color.yellow, 20000f);
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node4(walkable,worldPoint, x, y);
            }
        }
    }

    public List<Node4> GetNeighbours(Node4 node)
    {
        var neighbours = new List<Node4>();
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

    public Node4 NodeFromWorldPoint(Vector3 worldPosition) 
    {
        float percentX = worldPosition.x/gridWorldSize.x + 0.5f;
		float percentY = worldPosition.z/gridWorldSize.y + 0.5f;

        percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

		return grid[x,y];
	}
    
    void OnDrawGizmos() 
    {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
		if (grid != null && displayGridGizmos) 
        {
			foreach (Node4 n in grid) 
            {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
    
}
