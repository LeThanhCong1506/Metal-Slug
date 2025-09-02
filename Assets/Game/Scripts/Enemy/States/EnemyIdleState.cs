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

        idleTime = Random.Range(1f, 3f); 
        timer = 0f;

        controller.movementManager.StopMoving();
        controller.animationManager.StartLookStraightAnimation();
    }
    public override void Update()
    {
        timer += Time.deltaTime;
    }
}
