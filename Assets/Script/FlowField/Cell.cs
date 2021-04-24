using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 worldPos;
    public Vector2Int gridIndex;
    /// <summary>
    /// Composition of a grid cell
    /// </summary>
    /// <param name="_worldPos">Vector3 Position in the world</param>
    /// <param name="_gridIndex">"X"/"Y" Position in grid</param>
    public Cell(Vector3 _worldPos, Vector2Int _gridIndex)
    {
        worldPos = _worldPos;
        gridIndex = _gridIndex;
    }
}
