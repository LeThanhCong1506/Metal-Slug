using HealthSystem;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField]
    private MeleeAttack meleeAttack;
    [SerializeField]
    private PlayerExplosives playerExplosives;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private Transform shootPositionUp;
    [SerializeField]
    private Transform shootPositionDown;

    public LayerMask enemyLayer;
    private MovementManager movementManager;
    private AnimationManager animManager;

    private void Awake()
    {
        movementManager = GetComponent<MovementManager>();
        animManager = GetComponent<AnimationManager>();
    }
    public void Throw()
    {
        playerExplosives.ThrowGrenade(animManager);
    }
    public void Attack()
    {
        var detect = CheckAhead<Health>(meleeAttack.meleeRanged);
        if (detect != null)
        {
            MeleeAttack();
        }
        else
        {

            Shoot();
        }
    }

    private void MeleeAttack()
    {
        meleeAttack.AttackType1();
    }
    private void Shoot()
    {
        if (movementManager.LookDirection == LookDirection.Straight)
        {
            animManager.StartShootingAnimation();
            BulletController bullet = BulletManager.Instance.GetBullet();
            bullet.Active(shootPosition.position, new Vector2(transform.localScale.x, 0));
        }
        else if (movementManager.LookDirection == LookDirection.Up)
        {
            animManager.StartShootingUpAnimation();
            BulletController bullet = BulletManager.Instance.GetBullet();
            bullet.Active(shootPositionUp.position, new Vector2(0, 1));
        }
        else if (movementManager.LookDirection == LookDirection.Down)
        {
            animManager.StartShootingDownAnimation();
            BulletController bullet = BulletManager.Instance.GetBullet();
            bullet.Active(shootPositionDown.position, new Vector2(0, -1));
        }
    }

    public T CheckAhead<T>(float checkDistance) where T : Component
    {
        Vector3 dir = transform.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, checkDistance, enemyLayer);

        if (hit.collider != null)
        {
            // Thử lấy component T
            T component = hit.collider.GetComponent<T>();
            if (component != null)
            {
                return component; 
            }
        }

        return null; 
    }

}
