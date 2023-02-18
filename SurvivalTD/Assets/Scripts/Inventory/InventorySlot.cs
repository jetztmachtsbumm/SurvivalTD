using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private ItemStack itemStack;

    private GameObject selectedVisual;
    private Image contentImage;
    private TextMeshProUGUI amountText;

    private void Awake()
    {
        selectedVisual = transform.Find("Selected").gameObject;
        contentImage = transform.Find("Content").GetComponent<Image>();
        amountText = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
    }

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
            itemStack = null;
            MovingItem.Instance.gameObject.SetActive(true);
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

            itemStack = movingItemStack;
        }

        UpdateVisuals();
    }

    public ItemStack GetItemStack()
    {
        return itemStack;
    }

    public void SetItemStack(ItemStack itemStack)
    {
        this.itemStack = itemStack;
    }

}
