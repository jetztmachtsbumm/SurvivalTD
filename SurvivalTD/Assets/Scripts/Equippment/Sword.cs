using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IEquippment
{

    [SerializeField] private int damagePerHit;
    [SerializeField] private float attacksPerSecond;

    private bool canAttack;

    public void Use()
    {
        if (canAttack)
        {
            //Trigger animation
            StartCoroutine(AttackCooldown());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out BaseEnemy enemy))
        {
            enemy.GetHealthSystem().Damage(damagePerHit);
        }
    }

    private IEnumerator AttackCooldown()
    {
        while (!canAttack)
        {
            yield return new WaitForSeconds(1 / attacksPerSecond);
            canAttack = true;
        }
    }

}
