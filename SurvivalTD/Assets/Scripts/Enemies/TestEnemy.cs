using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : BaseEnemy
{

    [SerializeField] private Transform targetPosition;

    private List<GridPosition> path;
    private bool continueMoving;

    private void Start()
    {
        path = Pathfinding.Instance.FindPath(LevelGrid.Instance.GetGridPosition(transform.position), LevelGrid.Instance.GetGridPosition(targetPosition.position));
        continueMoving = true;
    }

    private void Update()
    {
        if (!continueMoving) return;

        Vector3 moveDirection = (LevelGrid.Instance.GetWorldPosition(path[0]) - transform.position).normalized;
        float stoppingDistance = 0.1f;

        if(Vector3.Distance(transform.position, LevelGrid.Instance.GetWorldPosition(path[0])) > stoppingDistance)
        {
            float moveSpeed = 8f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float rotationSpeed = 20f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed) + new Vector3(0, -90, 0);
        }
        else
        {
            path.Remove(path[0]);
            if(path.Count == 0)
            {
                continueMoving = false;
            }
        }
    }

}
