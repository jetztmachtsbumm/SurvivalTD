using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{

    public static PlayerInventory Instance { get; private set; }

    [SerializeField] private Transform hotbarUI;
    [SerializeField] private int hotbarSize;
    [SerializeField] private List<GameObject> playerEquipment;

    private InventorySlot[] hotBar;
    private int hotBarSelectedIndex;
    private GameObject activeEquippment;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            Debug.LogWarning("It seems like there's more than one PlayerInventory active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        SetupHotbar();
        ChangeHotBarSelectedIndex(0);
    }

    public void SpendItems(ItemCost itemCost)
    {
        foreach(ItemStack itemStack in itemCost.GetCost())
        {
            RemoveItem(itemStack);
        }
    }

    public void ChangeHotBarSelectedIndex(int index)
    {
        if (index >= hotbarSize)
        {
            return;
        }

        if(index < 0)
        {
            return;
        }

        hotBar[hotBarSelectedIndex].ToggleSelectedVisual(false);
        hotBarSelectedIndex = index;
        hotBar[index].ToggleSelectedVisual(true);

        ItemStack itemStack = hotBar[index].GetItemStack();
        activeEquippment?.SetActive(false);
        if (itemStack != null)
        {
            EquippableItemSO equippment = itemStack.item as EquippableItemSO;
            foreach(GameObject gameObject in playerEquipment)
            {
                if (gameObject.transform.name.Equals(equippment.gameObjectName))
                {
                    activeEquippment = gameObject;
                    break;
                }
            }
            activeEquippment?.SetActive(true);
            activeEquippment?.GetComponent<IEquippment>().Equip();
        }
    }

    public void ScrollHotBarUp()
    {
        if (hotBarSelectedIndex == hotbarSize - 1)
        {
            ChangeHotBarSelectedIndex(0);
        }
        else
        {
            ChangeHotBarSelectedIndex(hotBarSelectedIndex + 1);
        }
    }

    public void ScrollHotBarDown()
    {
        if (hotBarSelectedIndex == 0)
        {
            ChangeHotBarSelectedIndex(hotbarSize - 1);
        }
        else
        {
            ChangeHotBarSelectedIndex(hotBarSelectedIndex - 1);
        }
    }

    private void SetupHotbar()
    {
        hotBar = new InventorySlot[hotbarSize];
        for (int i = 0; i < hotBar.Length; i++)
        {
            hotBar[i] = Instantiate(inventoryCell, hotbarUI).GetComponent<InventorySlot>();
            hotBar[i].SetInventory(this);
            hotBar[i].SetIsHotbarSlot(true);
        }
    }

    public int GetHotbarSelectedIndex()
    {
        return hotBarSelectedIndex;
    }

    public GameObject GetActiveEquippment()
    {
        return activeEquippment;
    }

}
