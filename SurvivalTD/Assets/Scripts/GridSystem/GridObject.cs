using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private BuildingSO building;
    private ResourceNode resourceNode;
    private bool isOccupied;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        Collider[] collidersOnPosition = Physics.OverlapBox(gridSystem.GetWorldPosition(gridPosition), new Vector3(gridSystem.GetCellSize() / 2, 10f, gridSystem.GetCellSize() / 2));
        foreach (Collider collider in collidersOnPosition)
        {
            if (collider.transform.TryGetComponent(out ResourceNode resourceNode))
            {
                SetResourceNode(resourceNode);
                break;
            }
        }
    }

    public BuildingSO GetBuilding()
    {
        return building;
    }

    public ResourceNode GetResourceNode()
    {
        return resourceNode;
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
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

}
