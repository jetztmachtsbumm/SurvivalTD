using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunTurret : AttackingBuilding
{

    [SerializeField] private Transform rotateTowardsTarget;

    protected override void Update()
    {
        base.Update();

        if (target == null) return;

        float rotationSpeed = 5f;
        rotateTowardsTarget.rotation = Quaternion.Lerp(rotateTowardsTarget.rotation, Quaternion.LookRotation(target.transform.position - rotateTowardsTarget.position), rotationSpeed * Time.deltaTime);
    }

}
