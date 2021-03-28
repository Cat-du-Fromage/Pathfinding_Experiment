using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding5 : MonoBehaviour
{
    public Transform seeker, target;
    PathRequestManager requestManager;

    Grid5 grid;

    void Awake() 
    {
        grid = GetComponent<Grid5>();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            FindPath(seeker.position, target.position);
        }
    }
    
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //========================
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //========================

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
                    RetracePath(startNode, targetNode);
                    return;
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

                        if(!openSet.Contains(neighbour)) 
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Get all the node by retrieving each parent node previously store
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    void RetracePath(Node4 startNode, Node4 endNode)
    {
        var path = new List<Node4>();
        Node4 currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node4 nodeA, Node4 nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return (distX > distY) ? 14 * distY + 10 * (distX - distY) : 14 * distX + 10 * (distY - distX);
    }
}
