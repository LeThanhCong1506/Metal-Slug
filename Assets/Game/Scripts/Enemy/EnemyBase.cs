using Game.Scripts.Gameplay;
using Game.Service;
using UnityEngine;

[RequireComponent(typeof(MovementManager), typeof(AnimationManager), typeof(PhysicManager))]
public abstract class EnemyBase : MonoBehaviour
{
    public AnimationManager animationManager { get; private set; }
    public MovementManager movementManager { get; private set; }
    public PhysicManager physicManager { get; private set; }

    public IAttackStrategy AttackStrategy { get; protected set; }
    public IMoveStrategy MoveStrategy { get; protected set; }

    public StateMachine fsm;
    public Transform player;

    [SerializeField] protected float detectRange = 5f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected int health = 3;

    public bool IsDead => health <= 0;

    protected virtual void Awake()
    {
        animationManager = GetComponent<AnimationManager>();
        movementManager = GetComponent<MovementManager>();
        physicManager = GetComponent<PhysicManager>();
        player = FindFirstObjectByType<PlayerController>().transform;
        fsm = new StateMachine();
    }

    public bool CanSeePlayer()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) < detectRange;
    }

    public bool InAttackRange()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) < attackRange;
    }

    public virtual void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            fsm.ChangeState(new DieState(this));
        }
    }

    protected abstract void InitFSM();

    protected virtual void Start()
    {
        InitFSM();
    }

    protected virtual void Update()
    {
        fsm.Update();
    }
}
