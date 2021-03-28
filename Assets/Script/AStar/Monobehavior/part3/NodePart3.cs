using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePart3
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public NodePart3 parent;
    public int gCost; //distance from starting position
    public int hCost; //distance from target position
    public NodePart3(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get{return gCost + hCost;}
    }
}
