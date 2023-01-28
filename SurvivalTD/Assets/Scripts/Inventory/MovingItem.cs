using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovingItem : MonoBehaviour
{
    
    public static MovingItem Instance { get; private set; }

    private ItemStack itemStack;
    private Image image;
    private TextMeshProUGUI amountText;
    private Vector3 offset;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there's more than one MovingItem object active in the scene!");
        }
        Instance = this;

        image = transform.Find("Image").GetComponent<Image>();
        amountText = transform.Find("Amount").GetComponent<TextMeshProUGUI>();

        offset = new Vector3(10, 10, 0);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition + offset;
    }

    public ItemStack GetItemStack()
    {
        return itemStack;
    }

    public void SetItemStack(ItemStack itemStack)
    {
        this.itemStack = itemStack;
        image.sprite = itemStack.item.sprite;
        amountText.text = itemStack.amount.ToString();
    }

}
