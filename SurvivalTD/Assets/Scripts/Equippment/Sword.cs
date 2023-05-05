using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IEquippment
{

    [SerializeField] private int damagePerLightHit;
    [SerializeField] private int damagePerHeavyHit;
    [SerializeField] private float lightAttacksPerSecond;
    [SerializeField] private float heavyAttacksPerSecond;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip equipAnimationClip;

    private bool canAttack;
    private bool heavyAttack;
    private bool canDamage;

    private void Awake()
    {
        canDamage = false;
    }

    public void Equip()
    {
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Equip");
        canAttack = false;
        canDamage = false;
        StartCoroutine(AwaitEquipAnimationFinish(equipAnimationClip.length));
    }

    public void Use()
    {
        if (canAttack)
        {
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("LightAttack");
            canAttack = false;
            canDamage = true;
            StartCoroutine(AttackCooldown(false));
        }
    }

    public void AltUse()
    {
        if (canAttack)
        {
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("HeavyAttack");
            canAttack = false;
            canDamage = true;
            StartCoroutine(AttackCooldown(true));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canDamage)
        {
            if(other.TryGetComponent(out BaseEnemy enemy))
            {
                enemy.GetHealthSystem().Damage(heavyAttack ? damagePerHeavyHit : damagePerLightHit);
                canDamage = false;
            }
        }
    }

    private IEnumerator AttackCooldown(bool heavyAttack)
    {
        this.heavyAttack = heavyAttack;
        while (!canAttack)
        {
            if (heavyAttack)
            {
                yield return new WaitForSeconds(1 / heavyAttacksPerSecond);
            }
            else
            {
                yield return new WaitForSeconds(1 / lightAttacksPerSecond);
            }
            canAttack = true;
            this.heavyAttack = false;
            canDamage = false;
        }
    }

    private IEnumerator AwaitEquipAnimationFinish(float clipDuration)
    {
        while (true)
        {
            yield return new WaitForSeconds(clipDuration);
            canAttack = true;
            break;
        }
    }

}
