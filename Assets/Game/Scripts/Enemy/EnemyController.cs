using Game.Scripts.Gameplay;
using Game.Service;
using UnityEngine;

[RequireComponent(typeof(MovementManager))]
public class EnemyController : EnemyBase
{
    [Header("Detection Settings")]
    StateMachine m_StateMachine;

    void At(IState from, IState to, IPredicate condition)
    {
        m_StateMachine.AddTransition(from, to, condition);
    }
    void Any(IState to, IPredicate condition)
    {
        m_StateMachine.AddAnyTransition(to, condition);
    }

    protected override void InitFSM()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        m_StateMachine = new StateMachine();

        var idle = new EnemyIdleState(this);
        var patrol = new PatrolState(this);
        var hitted = new HitState(this);
        var chase = new ChaseState(this);
        var die = new DieState(this);

        Any(die, new FuncPredicate(() => IsDead));
        Any(hitted, new FuncPredicate(() => IsHitted));

        At(idle, patrol, new FuncPredicate(() => !InAttackRange()));
        At(patrol, chase, new FuncPredicate(CanSeePlayer));
        At(chase, patrol, new FuncPredicate(() => !CanSeePlayer()));
        At(chase, idle, new FuncPredicate(() => InAttackRange()));
        m_StateMachine.SetState(idle);
    }
}
