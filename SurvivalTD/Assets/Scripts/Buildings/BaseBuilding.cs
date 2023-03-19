using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public abstract class BaseBuilding : MonoBehaviour
{

    private HealthSystem healthSystem;

    protected virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnHealthZero += HealthSystem_OnHealthZero;
    }

    public virtual bool IsBuildingConditionMet(GridPosition gridPosition, out string error)
    {
        error = "";
        return true;
    }

    private void HealthSystem_OnHealthZero(object sender, System.EventArgs e)
    {
        healthSystem.OnHealthZero -= HealthSystem_OnHealthZero;
        Destroy(gameObject);
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

}
