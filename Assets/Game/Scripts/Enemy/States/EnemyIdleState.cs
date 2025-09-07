using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float idleTime;
    private float timer;

    public EnemyIdleState(EnemyBase controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("enter idle");

        controller.movementManager.StopMoving();
        controller.animationManager.StartLookStraightAnimation();
    }
}
public class AttackState : EnemyState
{
    private float attackCooldown = 1f;
    private float timer;

    public AttackState(EnemyBase controller) : base(controller)
    {
    }

    public void Enter()
    {
        timer = 0f;
        controller.movementManager.StopMoving();
    }

    public void Tick()
    {
        timer += Time.deltaTime;

        if (!controller.InAttackRange())
        {
            controller.fsm.ChangeState(new ChaseState(controller));
            return;
        }

        //if (timer >= attackCooldown)
        //{
        //    controller.AttackStrategy?.Attack(controller);
        //    timer = 0f;
        //}
    }

    public void Exit() { }
}

