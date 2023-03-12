using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnHealthZero;

    [SerializeField] private int maxHealth = 100;

    private int health;
    private Image healthBarImage;

    private void Awake()
    {
        health = maxHealth;
        healthBarImage = transform.Find("Bar").GetComponent<Image>();
    }

    public void Damage(int damage)
    {
        if (damage < health)
        {
            health -= damage;
            healthBarImage.fillAmount = GetHealthPercentage();
        }
        else
        {
            health = 0;
            healthBarImage.fillAmount = GetHealthPercentage();
            OnHealthZero?.Invoke(this, EventArgs.Empty);
        }
    }

    private float GetHealthPercentage()
    {
        return (float) health / maxHealth;
    }

}
