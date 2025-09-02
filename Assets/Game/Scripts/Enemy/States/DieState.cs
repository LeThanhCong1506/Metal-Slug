using UnityEngine;

public class DieState : EnemyState
{
    public DieState(EnemyBase controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("enter die");

        controller.movementManager.StopMoving();
        controller.animationManager.StopRunningAnimation();
        controller.GetComponent<Collider2D>().enabled = false;
        controller.enabled = false;
    }
}