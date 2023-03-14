using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackingBuilding : BaseBuilding
{

    [SerializeField] private float range;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private int damagePerHit;

    private BaseEnemy target;
    private bool attackingTarget;
    private Coroutine coroutine;

    public override abstract bool IsBuildingConditionMet(GridPosition gridPosition, out string error);

    private void Update()
    {
        if(target == null)
        {
            SeekTarget();
        }
        else
        {
            if(Vector3.Distance(transform.position, target.transform.position) > range)
            {
                ResetTarget();
                return;
            }

            //TODO: Rotate towards enemy
            if (!attackingTarget)
            {
                coroutine = StartCoroutine(AttackTarget());
                target.OnDeath += Target_OnDeath;
                attackingTarget = true;
            }
        }
    }

    private IEnumerator AttackTarget()
    {
        while (true)
        {
            target.GetHealthSystem().Damage(damagePerHit);
            yield return new WaitForSeconds(1 / shotsPerSecond);
        }
    }

    private void Target_OnDeath(object sender, System.EventArgs e)
    {
        ResetTarget();
    }

    private void SeekTarget()
    {
        List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
        //Get all enemies, that are within the attack range of the building
        foreach (Collider coll in Physics.OverlapSphere(transform.position, range))
        {
            if (coll.TryGetComponent(out BaseEnemy enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }

        if (enemiesInRange.Count == 0)
        {
            return;
        }

        //Determine nearest enemy
        BaseEnemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;
        foreach (BaseEnemy enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        target = nearestEnemy;
    }

    private void ResetTarget()
    {
        if(coroutine != null) StopCoroutine(coroutine);
        target.OnDeath -= Target_OnDeath;
        attackingTarget = false;
        target = null;
        coroutine = null;
    }

}
