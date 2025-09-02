using Game.Scripts.Gameplay;
using Game.Service;
using UnityEngine;

[RequireComponent(typeof(MovementManager))]
public class EnemyController : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] public Transform player;
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float attackRange = 1.5f;

    public AnimationManager animationManager { get; private set; }
    public MovementManager movementManager { get; private set; }
    public PhysicManager physicManager { get; private set; }

    StateMachine m_StateMachine;


    public bool IsDie { get; private set; }
    public bool IsHitted { get; private set; }

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        animationManager = GetComponent<AnimationManager>();
        movementManager = GetComponent<MovementManager>();
        physicManager = GetComponent<PhysicManager>();
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
        At(chase, idle, new  FuncPredicate(() => InAttackRange())); 
        m_StateMachine.SetState(idle);
    }

    void Update()
    {
        m_StateMachine.Update();
    }
    void At(IState from, IState to, IPredicate condition)
    {
        m_StateMachine.AddTransition(from, to, condition);
    }
    void Any(IState to, IPredicate condition)
    {
        m_StateMachine.AddAnyTransition(to, condition);
    }
    public bool CanSeePlayer()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) < detectRange;
    }
    public bool InAttackRange()
    {
        Debug.Log("in att0");
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) < attackRange;
    }
    public void ResetHitFlag()
    {
        IsHitted = false;
    }

}
