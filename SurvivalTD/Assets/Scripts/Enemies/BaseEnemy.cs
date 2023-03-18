using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public abstract class BaseEnemy : MonoBehaviour
{

    public event EventHandler OnDeath;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnHealthZero += HealthSystem_OnHealthZero;
    }

    private void HealthSystem_OnHealthZero(object sender, EventArgs e)
    {
        //Logic
        OnDeath?.Invoke(this, EventArgs.Empty);
        //DeathAnimation
        Destroy(gameObject);
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

}
