using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathfindingPart3 : MonoBehaviour
{
    public Transform seeker, target;
    GridPart3 grid;

    void Awake() 
    {
        grid = GetComponent<GridPart3>();
    }

    private void Update() 
    {
        FindPath(seeker.position, target.position);
    }
    
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        NodePart3 startNode = grid.NodeFromWorldPoint(startPos);
        NodePart3 targetNode = grid.NodeFromWorldPoint(targetPos);
        //List<NodePart3> OpenSet = new List<NodePart3>();
        var openSet = new List<NodePart3>();
        var closedSet = new HashSet<NodePart3>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            NodePart3 currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                {
                    if(openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode) 
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(NodePart3 neighbour in grid.GetNeighbours(currentNode)) // find the nearest valid node and add it to the opensetlist
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


    void RetracePath(NodePart3 startNode, NodePart3 endNode)
    {
        var path = new List<NodePart3>();
        NodePart3 currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(NodePart3 nodeA, NodePart3 nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return (distX > distY) ? 14 * distY + 10 * (distX - distY) : 14 * distX + 10 * (distY - distX);
    }
}
