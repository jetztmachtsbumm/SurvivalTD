using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private int slotCount;

    private Transform inventoryUI;
    private GameObject inventoryCell;
    private List<ItemStack> items;
    private List<InventorySlot> slots;

    protected virtual void Awake()
    {
        inventoryUI = GameObject.Find("Canvas").transform.Find("InventoryUI");
        inventoryCell = Resources.Load<GameObject>("InventoryCell");
        items = new List<ItemStack>();
    }

    public void AddItem(ItemStack itemStack)
    {
        foreach(ItemStack stack in items)
        {
            if(itemStack.item.name == stack.item.name)
            {
                stack.amount += itemStack.amount;
                UpdateSlots(stack);
                return;
            }
        }

        items.Add(itemStack);

        UpdateSlots(itemStack);
    }

    public void RemoveItem(ItemStack itemStack)
    {
        foreach(ItemStack stack in items)
        {
            if (itemStack.item.name == stack.item.name)
            {
                stack.amount -= itemStack.amount;
                if(stack.amount <= 0)
                {
                    items.Remove(stack);
                    UpdateSlots(stack);
                    return;
                }

                UpdateSlots(stack);
                return;
            }
        }
    }

    public void ToggleUI(bool on)
    {
        if (on)
        {
            SetupUI();
        }
        else
        {
            inventoryUI.gameObject.SetActive(false);
        }
    }

    private void UpdateSlots(ItemStack itemStack)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.GetItemStack()?.item.name == itemStack.item.name)
            {
                if(itemStack.amount <= 0)
                {
                    slot.SetItemStack(null);
                    slot.UpdateVisuals();
                    return;
                }
                slot.GetItemStack().amount = itemStack.amount;
                slot.UpdateVisuals();
                return;
            }
        }

        foreach(InventorySlot slot in slots)
        {
            if(slot.GetItemStack() == null)
            {
                slot.SetItemStack(itemStack);
                slot.UpdateVisuals();
                return;
            }
        }
    }

    private void SetupUI()
    {
        foreach(Transform transform in inventoryUI.Find("Scroll").Find("Panel").GetComponentsInChildren<Transform>())
        {
            if(transform.name != "Panel") Destroy(transform.gameObject);
        }

        slots = new List<InventorySlot>();

        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(Instantiate(inventoryCell, inventoryUI.Find("Scroll").Find("Panel")).GetComponent<InventorySlot>());
        }

        foreach(ItemStack itemStack in items)
        {
            UpdateSlots(itemStack);
        }

        inventoryUI.gameObject.SetActive(true);
    }

    public List<ItemStack> GetItems()
    {
        return items;
    }

    public List<InventorySlot> GetSlots()
    {
        return slots;
    }

}
