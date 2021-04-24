using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
    public GridDebug gridDebug;

    private void InitializeFlowField()
    {
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            InitializeFlowField();
        }
    }
}
