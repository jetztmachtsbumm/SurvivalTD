using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunTurret : AttackingBuilding
{

    public override bool IsBuildingConditionMet(GridPosition gridPosition, out string error)
    {
        error = "";
        return true;
    }

}
