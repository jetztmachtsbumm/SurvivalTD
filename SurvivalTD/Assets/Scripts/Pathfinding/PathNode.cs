using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{

    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode previousNode;
    private bool isWalkable = true;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ResetPreviousNode()
    {
        previousNode = null;
    }

    public void SetPreviousNode(PathNode node)
    {
        previousNode = node;
    }

    public int GetFCost()
    {
        return fCost;
    }

    public int GetGCost()
    {
        return gCost;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public PathNode GetPreviousPathNode()
    {
        return previousNode;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

}
