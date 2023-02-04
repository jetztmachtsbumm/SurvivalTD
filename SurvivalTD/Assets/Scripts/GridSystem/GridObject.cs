using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private BuildingSO building;
    private ResourceNode resourceNode;
    private bool isOccupied;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public BuildingSO GetBuilding()
    {
        return building;
    }

    public void SetBuilding(BuildingSO building)
    {
        this.building = building;
        if(building == null)
        {
            isOccupied = false;
        }
        else
        {
            isOccupied = true;
        }
    }

    public void SetResourceNode(ResourceNode resourceNode)
    {
        this.resourceNode = resourceNode;
        isOccupied = true;
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

}
