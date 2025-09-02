using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyController controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("enter chase");   

        controller.animationManager.StartRunningAnimation();
    }
    public override void Update()
    {
        if (controller == null || controller.movementManager == null || controller.player == null) return;

        Vector3 dir = (controller.player.position.x > controller.transform.position.x) ? Vector3.right : Vector3.left;
        controller.movementManager.HorizontalMovement(dir);
    }
    public override void OnExit()
    {
        controller.movementManager.StopMoving();
    }
}
