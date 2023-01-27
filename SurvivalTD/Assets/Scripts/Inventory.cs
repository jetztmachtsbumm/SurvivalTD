using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory Instance { get; private set; }

    [SerializeField] private GameObject inventoryCell;

    private List<InventoryCell> inventoryCells;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there's more than one Inventory active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        inventoryCells = new List<InventoryCell>();

        int invHeight = 5;
        int invWidth = 10;
        int startHeight = 120;
        int startWidth = -270;
        int offset = 60;

        for (int h = 0; h < invHeight; h++)
        {
            for (int w = 0; w < invWidth; w++)
            {
                GameObject cell = Instantiate(inventoryCell, transform);
                RectTransform rectTransform = cell.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = new Vector2(startWidth + (offset * w), startHeight - (offset * h));

                inventoryCells.Add(cell.GetComponent<InventoryCell>());
            }
        }
    }

    public void AddItem(ItemStack itemStack)
    {
        foreach(InventoryCell cell in inventoryCells)
        {
            if (cell.GetItemStack() == null) continue;

            if(cell.GetItemStack().item == itemStack.item)
            {
                cell.GetItemStack().amount += itemStack.amount;
                cell.UpdateVisuals();
                return;
            }
        }

        foreach(InventoryCell cell in inventoryCells)
        {
            if(cell.GetItemStack() == null)
            {
                cell.SetItemStack(itemStack);
                cell.UpdateVisuals();
                return;
            }
        }
    }

    public void RemoveItem(ItemStack itemStack)
    {
        foreach(InventoryCell cell in inventoryCells)
        {
            if (cell.GetItemStack() == null) continue;

            if(cell.GetItemStack().item == itemStack.item)
            {
                cell.GetItemStack().amount -= itemStack.amount;

                if(cell.GetItemStack().amount <= 0)
                {
                    cell.SetItemStack(null);
                }

                cell.UpdateVisuals();
                return;
            }
        }
    }

}
