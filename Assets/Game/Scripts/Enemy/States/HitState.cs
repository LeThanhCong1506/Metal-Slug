using UnityEngine;

public class HitState : EnemyState
{
    private float hitDuration = 0.3f;
    private float timer;
    public HitState(EnemyBase controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        Debug.Log("enter hit");

        timer = 0f;
        controller.movementManager.StopMoving();
    }
    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= hitDuration)
        {
            controller.ResetHitFlag();
        }
    }
    public override void OnExit()
    {
    }
}
