using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField]
    private MeleeAttack meleeAttack;
    [SerializeField]
    private Transform shootPosition;
    [SerializeField]
    private Transform shootPositionUp;
    [SerializeField]
    private Transform shootPositionDown;

    private MovementManager movementManager;
    private AnimationManager animManager;

    private void Awake()
    {
        movementManager = GetComponent<MovementManager>();
        animManager = GetComponent<AnimationManager>();
    }

    public void Attack()
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
}
