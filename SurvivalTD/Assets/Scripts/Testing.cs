using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    [SerializeField] private ItemSO item;
    [SerializeField] private HealthSystem healthSystem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerInventory.Instance.AddItem(new ItemStack { item = item, amount = 1 }, null);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerInventory.Instance.RemoveItem(new ItemStack { item = item, amount = 1 });
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            healthSystem.Damage(37);
        }
    }

}
