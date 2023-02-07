using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    [SerializeField] private ItemSO item;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerInventory.Instance.AddItem(new ItemStack { item = item, amount = 1 });
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerInventory.Instance.RemoveItem(new ItemStack { item = item, amount = 1 });
        }
    }

}
