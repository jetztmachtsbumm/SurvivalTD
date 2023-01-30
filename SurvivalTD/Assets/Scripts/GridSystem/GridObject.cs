using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private BuildingSO building;

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
    }

}
