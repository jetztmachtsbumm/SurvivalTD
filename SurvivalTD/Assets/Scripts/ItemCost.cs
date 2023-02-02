using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCost
{

    [SerializeField] private ItemStack[] cost;

    public ItemStack[] GetCost()
    {
        return cost;
    }

    public bool CanAfford(ItemStack[] availableItems)
    {
        if(availableItems.Length < cost.Length) return false;

        int costMet = 0;
        for(int i = 0; i < cost.Length; i++)
        {
            foreach(ItemStack item in availableItems)
            {
                if (cost[i].item == item.item && cost[i].amount <= item.amount)
                {
                    costMet++;
                }
            }
        }

        return costMet >= cost.Length;
    }

}
