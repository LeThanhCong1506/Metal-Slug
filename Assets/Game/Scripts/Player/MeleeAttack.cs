using HealthSystem;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField]
    private AnimationManager animManager;
    [SerializeField]
    private CapsuleCollider2D attackDetect;
    [SerializeField]
    private ContactFilter2D enemyFilter;


    private static readonly int IsAttacking = Animator.StringToHash("knifeing");
    private static readonly int Attack = Animator.StringToHash("knife");
    private static readonly int Attack2 = Animator.StringToHash("knife2");

    private bool isAttacking;


    public void AttackType1()
    {
        isAttacking = animManager.TopAnimator.GetBool(IsAttacking);
        if (isAttacking) return;
        Debug.Log("AttackType1");
        animManager.TopAnimator.SetTrigger(Attack);
    }
    public void AttackType2()
    {
        isAttacking = animManager.TopAnimator.GetBool(IsAttacking);
        if (isAttacking) return;
        animManager.TopAnimator.SetTrigger(Attack2);
    }

    public void DoAttack()
    {
        List<Collider2D> results = new List<Collider2D>();
        int count = Physics2D.OverlapCollider(attackDetect, enemyFilter, results);

        Debug.Log("result: " +count);
        for (int i = 0; i < count; i++)
        {
            
            var obtacle = results[i].GetComponent<Health>();
            if (obtacle != null)
            {
                Debug.Log("DoAttack");
                obtacle.TakeDamage(new DamageInfo(50));
            }
        }
    }
}
