using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingInventory : Inventory
{

    private TextMeshProUGUI title;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetBuildingInventoryUI(Transform buildingInventoryUI)
    {
        inventoryUI = buildingInventoryUI;
        title = inventoryUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTitle(string titleString)
    {
        title.text = titleString;
    }

}
