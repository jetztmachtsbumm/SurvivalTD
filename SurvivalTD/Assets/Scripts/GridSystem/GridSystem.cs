using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{

    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridObjects;

    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjects = new TGridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjects[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize));
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjects[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x > 0 &&
            gridPosition.z > 0 &&
            gridPosition.x < width &&
            gridPosition.z < height;
    }

    public bool IsGridPositionOccupied(GridPosition gridPosition)
    {
        return (GetGridObject(gridPosition) as GridObject).IsOccupied();
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

}
