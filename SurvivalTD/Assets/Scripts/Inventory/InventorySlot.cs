using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] private GameObject selectedVisual;
    [SerializeField] private Image contentImage;
    [SerializeField] private TextMeshProUGUI amountText;

    private ItemStack itemStack;
    private Inventory inventory;

    public void UpdateVisuals()
    {
        if(itemStack == null)
        {
            contentImage.gameObject.SetActive(false);
            amountText.text = "";
            return;
        }

        contentImage.gameObject.SetActive(true);
        contentImage.sprite = itemStack.item.sprite;
        amountText.text = itemStack.amount.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedVisual.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedVisual.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!MovingItem.Instance.gameObject.activeInHierarchy)
        {
            if (itemStack == null) return;

            MovingItem.Instance.SetItemStack(itemStack);
            int amount = MovingItem.Instance.GetItemStack().amount;
            inventory.RemoveItem(itemStack);
            MovingItem.Instance.gameObject.SetActive(true);
            MovingItem.Instance.GetItemStack().amount = amount;
        }
        else
        {
            ItemStack movingItemStack = MovingItem.Instance.GetItemStack();

            if(itemStack != null)
            {
                MovingItem.Instance.SetItemStack(itemStack);
            }
            else
            {
                MovingItem.Instance.gameObject.SetActive(false);
            }

            inventory.AddItem(movingItemStack, this);
        }
    }

    public ItemStack GetItemStack()
    {
        return itemStack;
    }

    public void SetItemStack(ItemStack itemStack)
    {
        this.itemStack = itemStack;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

}
