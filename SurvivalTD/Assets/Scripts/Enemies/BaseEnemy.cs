using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public abstract class BaseEnemy : MonoBehaviour
{

    public event EventHandler OnDeath;

    [SerializeField] private float targetingRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attacksPerSecond;
    [SerializeField] private Animator animator;

    private HealthSystem healthSystem;
    private Transform targetPosition;
    private List<GridPosition> path;
    private BaseBuilding targetBuilding;
    private bool continueMoving;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnHealthZero += HealthSystem_OnHealthZero;
        continueMoving = true;
    }

    private void Update()
    {
        if (!continueMoving)
        {
            animator.SetBool("Moving", false);
            return;
        }

        LookForNewTargets();
        MoveToNextTargetPosition();
    }

    private void HealthSystem_OnHealthZero(object sender, EventArgs e)
    {
        //Logic
        OnDeath?.Invoke(this, EventArgs.Empty);
        //DeathAnimation
        Destroy(gameObject);
    }

    private void MoveToNextTargetPosition()
    {
        Vector3 moveDirection = (LevelGrid.Instance.GetWorldPosition(path[0]) - transform.position).normalized;
        float stoppingDistance = 0.1f;

        if (Vector3.Distance(transform.position, LevelGrid.Instance.GetWorldPosition(path[0])) > stoppingDistance)
        {
            animator.SetBool("Moving", true);

            float moveSpeed = 8f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float rotationSpeed = 20f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }
        else
        {
            path.Remove(path[0]);
            if (path.Count == 0)
            {
                continueMoving = false;
                StartCoroutine(AttackBuilding());
            }
        }
    }

    private void LookForNewTargets()
    {
        Transform nearestTarget;
        if (targetPosition == null)
        {
            nearestTarget = MainBuilding.Instance.transform;
        }
        else
        {
            nearestTarget = targetPosition;
        }

        foreach(Collider coll in Physics.OverlapSphere(transform.position, targetingRange))
        {
            if(coll.TryGetComponent(out BaseBuilding building))
            {
                if (nearestTarget == null)
                {
                    nearestTarget = coll.transform;
                    continue;
                }

                if(Vector3.Distance(transform.position, coll.transform.position) < Vector3.Distance(transform.position, nearestTarget.position))
                {
                    nearestTarget = coll.transform;
                }
            }
        }

        if (nearestTarget != targetPosition)
        {
            targetPosition = nearestTarget;
            targetBuilding = targetPosition.GetComponent<BaseBuilding>();
            path = Pathfinding.Instance.FindPath(LevelGrid.Instance.GetGridPosition(transform.position), LevelGrid.Instance.GetGridPosition(targetPosition.position));
        }
    }

    private IEnumerator AttackBuilding()
    {
        while(targetBuilding != null)
        {
            //Animate
            targetBuilding.GetHealthSystem().Damage(attackDamage);
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(1 / attacksPerSecond);
        }

        //Building destroyed
        targetBuilding = MainBuilding.Instance;
        continueMoving = true;
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

}
