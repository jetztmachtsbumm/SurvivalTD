using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{

    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjects;

    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjects = new GridObject[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GridObject gridObject = new GridObject(this, gridPosition);

                Collider[] colliderOnPosition = Physics.OverlapBox(GetWorldPosition(gridPosition), new Vector3(cellSize / 2, 10f, cellSize / 2));
                foreach(Collider collider in colliderOnPosition)
                {
                    if(collider.transform.TryGetComponent(out ResourceNode resourceNode))
                    {
                        gridObject.SetResourceNode(resourceNode);
                        break;
                    }
                }

                gridObjects[x, z] = gridObject;
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

    public GridObject GetGridObject(GridPosition gridPosition)
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
        return GetGridObject(gridPosition).IsOccupied();
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

}
