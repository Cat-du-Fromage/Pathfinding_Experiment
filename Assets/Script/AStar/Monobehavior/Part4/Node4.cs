using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node4: IHeapItem<Node4>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public Node4 parent;
    public int gCost; //distance from starting position
    public int hCost; //distance from target position
    int heapIndex;
    public Node4(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
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

    public int HeapIndex
    {
        get {return heapIndex;}
        set {heapIndex = value;}
    }
    public int CompareTo(Node4 nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
