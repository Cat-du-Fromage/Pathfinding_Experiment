using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHeat
{
    public int width { get; set; }
    //int width
    public int height { get; set; }
    //int height;
    float cellSize;
    //int[,] gridArray;

    public int[,] gridArray { get; set; }

    public GridHeat(int _width, int _height, float _cellSize)
    {
        width = _width;
        height = _height;
        cellSize = _cellSize;

        gridArray = new int[width, height];
        Debug.Log($"width = {width}; height = {height}");

        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {

            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, 1, y * cellSize);
    }
}
