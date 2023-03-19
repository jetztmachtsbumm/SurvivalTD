using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBuilding : BaseBuilding, IInteractable
{

    protected BuildingInventory inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<BuildingInventory>();
    }

    public override abstract bool IsBuildingConditionMet(GridPosition gridPosition, out string error);

    public abstract string GetBuildingName();

    public void OnInteraction()
    {
        inventory.SetTitle(GetBuildingName());
        inventory.ToggleUI(true);
        PlayerInventory.Instance.ToggleUI(true);
    }

}
