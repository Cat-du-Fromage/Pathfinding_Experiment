using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding4 : MonoBehaviour
{
    //public Transform seeker, target;
    PathRequestManager requestManager;

    Grid4 grid;

    void Awake() 
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid4>();
    }
    /*
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            //FindPath(seeker.position, target.position);
        }
    }
    */
    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }
    
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //========================
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //========================
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node4 startNode = grid.NodeFromWorldPoint(startPos);
        Node4 targetNode = grid.NodeFromWorldPoint(targetPos);

        if(startNode.walkable && targetNode.walkable)
        {
            var openSet = new Heap4<Node4>(grid.MaxSize);
            var closedSet = new HashSet<Node4>();

            openSet.Add(startNode);
            while(openSet.Count > 0)
            {
                Node4 currentNode = openSet.RemoveFirst(); //starting node
                closedSet.Add(currentNode);

                if(currentNode == targetNode) 
                {
                    //========================
                    sw.Stop();
                    print($"Path found: {sw.ElapsedMilliseconds} ms");
                    //========================
                    pathSuccess = true;
                    //RetracePath(startNode, targetNode);
                    break;
                    //return;
                }

                foreach(Node4 neighbour in grid.GetNeighbours(currentNode)) // find the nearest valid node and add it to the opensetlist
                {
                    if(!neighbour.walkable || closedSet.Contains(neighbour)) {continue;} // check if not walkable or not part of list closedSet
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour); // calcul length beetween position and adjacent
                    if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode); 
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        yield return null;
        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    /// <summary>
    /// Get all the node by retrieving each parent node previously store
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    Vector3[] RetracePath(Node4 startNode, Node4 endNode)
    {
        var path = new List<Node4>();
        Node4 currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node4> path) 
    {
		var waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) 
        {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) 
            {
                // Also of importance: 
                // We start at i = 1, but we need to check all the way to the origin! so [i-1]
                waypoints.Add(path[i - 1].worldPosition);
            }
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

    int GetDistance(Node4 nodeA, Node4 nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return (distX > distY) ? 14 * distY + 10 * (distX - distY) : 14 * distX + 10 * (distY - distX);
    }
}
