using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public ItemStack GetItemStack()
    {
        return itemStack;
    }

    public void SetItemStack(ItemStack itemStack)
    {
        this.itemStack = itemStack;
    }

}
