using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Container
{

    public static PlayerInventory Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            Debug.LogWarning("It seems like there's more than one Inventory active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void SpendItems(ItemCost itemCost)
    {
        foreach(ItemStack itemStack in itemCost.GetCost())
        {
            RemoveItem(itemStack);
        }
    }

}
