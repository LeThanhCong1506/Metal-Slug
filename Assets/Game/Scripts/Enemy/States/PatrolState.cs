using UnityEngine;

public class PatrolState : EnemyState
{
    private float patrolSpeed = 1.5f;
    private float timer;
    private Vector3 dir = Vector3.right;
    public PatrolState(EnemyBase controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("enter patrol");
        controller.animationManager.StartRunningAnimation();
    }
    public override void Update()
    {
        controller.movementManager.HorizontalMovement(dir);

        timer += Time.deltaTime;
        if (timer > patrolSpeed)
        {
            timer = 0;
            dir = -dir;
        }
        
    }
    public override void OnExit()
    {
        controller.movementManager.StopMoving();
    }
}
