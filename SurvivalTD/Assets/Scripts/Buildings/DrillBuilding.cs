using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBuilding : BaseBuilding
{

    private ResourceNode resourceNode;
    private float timer;

    private void Start()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        resourceNode = LevelGrid.Instance.GetGridObject(gridPosition).GetResourceNode();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Inventory.Instance.AddItem(new ItemStack() { item = resourceNode.GetResource(), amount = resourceNode.GetResourcesPerSecond() });
            timer = 1;
        }
    }

    public override bool IsBuildingConditionMet(GridPosition gridPosition, out string error)
    {
        if(LevelGrid.Instance.GetGridObject(gridPosition).GetResourceNode() == null)
        {
            error = "Must be placed on Resourcenode!";
            return false;
        }

        error = "";
        return true;
    }

}
