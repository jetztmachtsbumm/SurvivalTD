using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnHealthZero;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float healthBarOffset;
    [SerializeField] private float healthBarSize = 1f;

    private int health;
    private Image healthBarImage;

    private void Awake()
    {
        health = maxHealth;
        Transform healthSystemObject = Instantiate(Resources.Load<Transform>("HealthSystem"), transform);
        healthSystemObject.transform.position += Vector3.up * healthBarOffset;
        healthSystemObject.GetComponent<RectTransform>().localScale = new Vector3(healthBarSize, 1, 1);
        healthBarImage = healthSystemObject.Find("Bar").GetComponent<Image>();
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
