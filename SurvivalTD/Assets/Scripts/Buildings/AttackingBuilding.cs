using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackingBuilding : BaseBuilding
{

    [SerializeField] private Transform shootPoint;
    [SerializeField] private float range;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private int damagePerHit;

    private BaseEnemy target;
    private bool attackingTarget;
    private Coroutine coroutine;

    protected override void Awake()
    {
        base.Awake();
        GetHealthSystem().OnHealthZero += AttackingBuilding_OnHealthZero;
    }

    private void Update()
    {
        if(target == null)
        {
            SeekTarget();
        }
        else
        {
            if(Vector3.Distance(shootPoint.position, target.transform.position) > range)
            {
                ResetTarget();
                return;
            }

            Physics.Raycast(shootPoint.position, target.transform.position - shootPoint.position, out RaycastHit hit);

            if(!ReferenceEquals(hit.transform, target.transform))
            {
                ResetTarget();
                return;
            }

            //TODO: Rotate towards enemy

            if (!attackingTarget)
            {
                //Target sometimes null for unknown reason
                if(target == null) return;

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

    private void AttackingBuilding_OnHealthZero(object sender, System.EventArgs e)
    {
        if(target != null)
        {
            target.OnDeath -= Target_OnDeath;
        }
    }

    private void Target_OnDeath(object sender, System.EventArgs e)
    {
        //Target sometimes null for unknown reason
        if(target != null)
        {
            target.OnDeath -= Target_OnDeath;
        }

        //Event is sometimes called after this gameobject got destroyed already
        if (this != null)
        {
            ResetTarget();
        }
    }

    private void SeekTarget()
    {
        List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
        //Get all enemies, that are within the attack range of the building
        foreach (Collider coll in Physics.OverlapSphere(shootPoint.position, range))
        {
            if (coll.TryGetComponent(out BaseEnemy enemy))
            {
                Physics.Raycast(shootPoint.position, coll.transform.position - shootPoint.position, out RaycastHit hit);
                Debug.DrawRay(shootPoint.position, coll.transform.position - shootPoint.position);
                if (hit.transform.TryGetComponent(out BaseEnemy baseEnemy))
                {
                    enemiesInRange.Add(enemy);
                }
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
            float distanceToEnemy = Vector3.Distance(shootPoint.position, enemy.transform.position);
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
        attackingTarget = false;
        target = null;
        coroutine = null;
    }

}
