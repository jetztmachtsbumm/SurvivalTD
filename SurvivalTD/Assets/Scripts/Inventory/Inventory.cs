using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{

    public static bool isAnyInventoryOn { get; private set; }

    [SerializeField] private int slotCount;
    [SerializeField] protected Transform inventoryUI;

    protected GameObject inventoryCell;
    private List<ItemStack> items;
    private List<InventorySlot> slots;

    protected virtual void Awake()
    {
        inventoryCell = Resources.Load<GameObject>("InventorySlot");
        items = new List<ItemStack>();
    }

    public void AddItem(ItemStack itemStack, InventorySlot slot)
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

        if (slot == null)
        {
            UpdateSlots(itemStack);
        }
        else
        {
            foreach(InventorySlot inventorySlot in slots)
            {
                if(inventorySlot == slot)
                {
                    inventorySlot.SetItemStack(itemStack);
                    inventorySlot.UpdateVisuals();
                    return;
                }
            }
        }
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            inventoryUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        isAnyInventoryOn = on;
    }

    private void UpdateSlots(ItemStack itemStack)
    {
        if(slots == null) return;

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
            InventorySlot slot = Instantiate(inventoryCell, inventoryUI.Find("Scroll").Find("Panel")).GetComponent<InventorySlot>();
            slot.SetInventory(this);
            slots.Add(slot);
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
