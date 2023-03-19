using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : BaseBuilding
{

    public static MainBuilding Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there is more than one MainBuilding object active in the scene!");
            Destroy(gameObject);
        }

        Instance = this;
    }

}
