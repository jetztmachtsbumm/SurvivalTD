using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Container : MonoBehaviour
{

    [SerializeField] private Transform containerUI;
    [SerializeField] private int invHeight = 5;
    [SerializeField] private int invWidth = 10;
    [SerializeField] private int startHeight = 120;
    [SerializeField] private int startWidth = -270;
    [SerializeField] private int offset = 60;

    private GameObject inventoryCell;
    private List<ContainerCell> inventoryCells;
    private Transform containerUITransform;

    protected virtual void Awake()
    {
        inventoryCell = Resources.Load<GameObject>("InventoryCell");
        inventoryCells = new List<ContainerCell>();

        Transform containerTransform = new GameObject(transform.name + " Container").transform;
        containerTransform.SetParent(containerUI);
        containerTransform.transform.localPosition = Vector3.zero;

        for (int h = 0; h < invHeight; h++)
        {
            for (int w = 0; w < invWidth; w++)
            {
                GameObject cell = Instantiate(inventoryCell, containerTransform);
                RectTransform rectTransform = cell.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = new Vector2(startWidth + (offset * w), startHeight - (offset * h));

                inventoryCells.Add(cell.GetComponent<ContainerCell>());
            }
        }

        containerUITransform = containerTransform;
        containerTransform.gameObject.SetActive(false);
    }

    public void AddItem(ItemStack itemStack)
    {
        foreach (ContainerCell cell in inventoryCells)
        {
            if (cell.GetItemStack() == null) continue;

            if (cell.GetItemStack().item == itemStack.item)
            {
                cell.GetItemStack().amount += itemStack.amount;
                cell.UpdateVisuals();
                return;
            }
        }

        foreach (ContainerCell cell in inventoryCells)
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
        foreach (ContainerCell cell in inventoryCells)
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
        foreach (ContainerCell cell in inventoryCells)
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
        containerUI.Find("Background").gameObject.SetActive(on);
    }

}
