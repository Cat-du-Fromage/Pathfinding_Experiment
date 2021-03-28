using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingHeat : MonoBehaviour
{
    GridHeat grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridHeat(4, 2, 10f);
    }

    void OnDrawGizmos()
    {
        if(grid != null)
        {
            for (int x = 0; x < grid.gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < grid.gridArray.GetLength(1); y++)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1));
                    Gizmos.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y));
                }
            }
            Gizmos.DrawLine(grid.GetWorldPosition(0,grid.height), grid.GetWorldPosition(grid.width,grid.height));
            Gizmos.DrawLine(grid.GetWorldPosition(grid.width, 0), grid.GetWorldPosition(grid.width, grid.height));
        }
    }
}
