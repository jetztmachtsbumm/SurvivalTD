using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private GameObject selected;

    private void Awake()
    {
        selected = transform.Find("Selected").gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selected.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected.SetActive(false);
    }

}
