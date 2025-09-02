using Game.Scripts.Gameplay;
using Game.Service;
using UnityEngine;

[RequireComponent(typeof(MovementManager))]
public class EnemyController : EnemyBase
{
    [Header("Detection Settings")]
    [SerializeField] public Transform player;

    StateMachine m_StateMachine;


    public bool IsDie { get; private set; }
    public bool IsHitted { get; private set; }

    void At(IState from, IState to, IPredicate condition)
    {
        m_StateMachine.AddTransition(from, to, condition);
    }
    void Any(IState to, IPredicate condition)
    {
        m_StateMachine.AddAnyTransition(to, condition);
    }
    public void ResetHitFlag()
    {
        IsHitted = false;
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

        Any(die, new FuncPredicate(() => IsDie));
        Any(hitted, new FuncPredicate(() => IsHitted));

        At(idle, patrol, new FuncPredicate(() => !InAttackRange())); // idle -> patrol sau 1 tick
        At(patrol, chase, new FuncPredicate(CanSeePlayer));
        At(chase, patrol, new FuncPredicate(() => !CanSeePlayer()));
        At(chase, idle, new FuncPredicate(() => InAttackRange()));
        m_StateMachine.SetState(idle);
    }
}
