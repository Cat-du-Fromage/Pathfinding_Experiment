
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public GameObject terrain;
    public LayerMask unwalkableMask;
    Vector2 gridWorldSize;
    Mesh TerrainMesh;
    Bounds TerrainBounds;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start() 
    {
        TerrainMesh = terrain.GetComponent<MeshFilter>().mesh;
        TerrainBounds = TerrainMesh.bounds;
        gridWorldSize = new Vector2(TerrainBounds.size.x * terrain.transform.localScale.x,
                                    TerrainBounds.size.z * terrain.transform.localScale.z);
                                    //Debug.Log($"SizeX = {TerrainBounds.size.x * terrain.transform.localScale.x}"); // =100 OK
                                    //Debug.Log($"SizeZ = {TerrainBounds.size.z* terrain.transform.localScale.z}"); // =100 OK
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter); //grid Height
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter); //grid width
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2; //forward = z axis // right = x axis // up = y axis
        Debug.DrawLine(worldBottomLeft, worldBottomLeft + Vector3.up*20, Color.yellow, 20000f);
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
                /*
                Debug.DrawLine(worldPoint, worldPoint + Vector3.up*nodeRadius, Color.yellow, 20000f);
                Debug.Log($"NodeRadius = {nodeRadius}");
                */
				grid[x,y] = new Node(walkable,worldPoint);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition) 
    {
		//float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x; 
        // (a + b/2) / b = a/b + (b/2)/b
        //(b/2) / b/1 == b/2 * 1/b == 1/2
        //=> == a/b + 1/2
        //float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
        //Debug.Log($"worldPosition.x = {worldPosition.x}");
        //Debug.Log($"worldPosition.y = {worldPosition.y}");
        float percentX = worldPosition.x/gridWorldSize.x + 0.5f;
		float percentY = worldPosition.z/gridWorldSize.y + 0.5f;

        //Debug.Log($"percentX No Clamp = {percentX}");
        //Debug.Log($"percentY No Clamp = {percentY}");

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
        
        //Debug.Log($"percentX = {percentX}");
        //Debug.Log($"percentY = {percentY}");
		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
        /*
        Debug.Log($"x = {gridSizeX-1}");
        Debug.Log($"y = {gridSizeY-1}");
        Debug.Log($"x = {x}");
        Debug.Log($"y = {y}");
        */
        //int x = Mathf.Clamp(Mathf.RoundToInt((gridSizeX) * percentX), 0, gridSizeX-1);
		//int y = Mathf.Clamp(Mathf.RoundToInt((gridSizeY) * percentY), 0, gridSizeY-1);
		return grid[x,y];
	}

    void OnDrawGizmos() 
    {
        /*
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2; //forward = z axis // right = x axis // up = y axis
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(worldPoint,nodeRadius);
				grid[x,y] = new Node(walkable,worldPoint);
            }
        }
*/
       Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); //y on z is normal
       if(grid != null)
       {
           Node playerNode = NodeFromWorldPoint(player.position);
           foreach (Node n in grid)
           {
               Gizmos.color = (n.walkable) ? Color.white : Color.red;
                // careful, nee to precise it's the world position we are searching!
                //AND it seems to not considere default as a valide mask
               if(playerNode == n)
               {
                   Gizmos.color = Color.cyan;
               }
            
               Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f)); //Vector3.one = Vector3(1,1,1)
           }
       }
    }
}
