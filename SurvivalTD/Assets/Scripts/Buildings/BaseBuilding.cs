using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{

    public abstract bool IsBuildingConditionMet(GridPosition gridPosition, out string error);

}
