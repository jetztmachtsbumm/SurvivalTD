using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{

    public static Inventory Instance { get; private set; }

    [SerializeField] private GameObject inventoryCell;

    private Dictionary<GameObject, ItemStack> items;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there's more than one Inventory active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        items = new Dictionary<GameObject, ItemStack>();

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

                items[cell] = null;
            }
        }
    }

    public void AddItem(ItemStack item)
    {
        foreach(ItemStack itemStack in items.Values)
        {
            if (itemStack == null) continue;
            
            if(item.item == itemStack.item)
            {
                itemStack.amount += item.amount;
                UpdateVisuals();
                return;
            }
        }

        foreach(GameObject cell in items.Keys)
        {
            if (items[cell] == null)
            {
                items[cell] = item;
                UpdateVisuals();
                return;
            }
        }
    }

    private void UpdateVisuals()
    {
        foreach(GameObject gameObject in items.Keys)
        {
            Image contentImage = gameObject.transform.Find("Content").GetComponent<Image>();
            TextMeshProUGUI text = gameObject.transform.Find("Amount").GetComponent<TextMeshProUGUI>();

            if (items[gameObject] == null)
            {
                contentImage.gameObject.SetActive(false);
                text.text = "";
                return;
            }

            ItemStack itemStack = items[gameObject];

            contentImage.gameObject.SetActive(true);

            contentImage.sprite = itemStack.item.sprite;
            text.text = itemStack.amount.ToString();
        }
    }

}
