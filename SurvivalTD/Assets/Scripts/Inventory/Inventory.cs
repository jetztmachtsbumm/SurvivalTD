using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private int invHeight = 5;
    [SerializeField] private int invWidth = 10;
    [SerializeField] private int startHeight = 120;
    [SerializeField] private int startWidth = -270;
    [SerializeField] private int offset = 60;

    private Transform inventoryUI;
    private GameObject inventoryCell;
    private List<InventoryCell> inventoryCells;
    private Transform containerUITransform;

    protected virtual void Awake()
    {
        inventoryUI = GameObject.Find("Canvas").transform.Find("InventoryUI");
        inventoryCell = Resources.Load<GameObject>("InventoryCell");
        inventoryCells = new List<InventoryCell>();

        Transform containerTransform = new GameObject(transform.name + " Container").transform;
        containerTransform.SetParent(inventoryUI);
        containerTransform.transform.localPosition = Vector3.zero;

        for (int h = 0; h < invHeight; h++)
        {
            for (int w = 0; w < invWidth; w++)
            {
                GameObject cell = Instantiate(inventoryCell, containerTransform);
                RectTransform rectTransform = cell.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = new Vector2(startWidth + (offset * w), startHeight - (offset * h));

                inventoryCells.Add(cell.GetComponent<InventoryCell>());
            }
        }

        containerUITransform = containerTransform;
        containerTransform.gameObject.SetActive(false);
    }

    public void AddItem(ItemStack itemStack)
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            if (cell.GetItemStack() == null) continue;

            if (cell.GetItemStack().item == itemStack.item)
            {
                cell.GetItemStack().amount += itemStack.amount;
                cell.UpdateVisuals();
                return;
            }
        }

        foreach (InventoryCell cell in inventoryCells)
        {
            if (cell.GetItemStack() == null)
            {
                cell.SetItemStack(itemStack);
                cell.UpdateVisuals();
                return;
            }
        }
    }

    public void RemoveItem(ItemStack itemStack)
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            if (cell.GetItemStack() == null) continue;

            if (cell.GetItemStack().item == itemStack.item)
            {
                cell.GetItemStack().amount -= itemStack.amount;

                if (cell.GetItemStack().amount <= 0)
                {
                    cell.SetItemStack(null);
                }

                cell.UpdateVisuals();
                return;
            }
        }
    }

    public ItemStack[] GetAllItems()
    {
        List<ItemStack> items = new List<ItemStack>();
        foreach (InventoryCell cell in inventoryCells)
        {
            if (cell.GetItemStack() != null)
            {
                items.Add(cell.GetItemStack());
            }
        }

        return items.ToArray();
    }

    public void ToggleUI(out bool on)
    {
        containerUITransform.gameObject.SetActive(!containerUITransform.gameObject.activeInHierarchy);
        on = containerUITransform.gameObject.activeInHierarchy;
        inventoryUI.Find("Background").gameObject.SetActive(on);
    }

}
