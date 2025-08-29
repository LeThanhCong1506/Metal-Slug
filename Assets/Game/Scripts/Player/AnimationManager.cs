using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static readonly int IsRun = Animator.StringToHash("horizontal_pressed");
    private static readonly int LookDown = Animator.StringToHash("down_pressed");
    private static readonly int LookUp = Animator.StringToHash("up_pressed");
    private static readonly int IsJump = Animator.StringToHash("jump_trigger");
    private static readonly int IsJumpHigh = Animator.StringToHash("jump_high_speed");
    private static readonly int IsJumpLow = Animator.StringToHash("jump_low_speed");
    private static readonly int IsHitGround = Animator.StringToHash("hit_ground");

    [SerializeField] private Animator topAnimator;
    [SerializeField] private Animator bottomAnimator;

    public Animator TopAnimator => topAnimator;

    void OnEnable()
    {
        PlayerEvents.OnPlayerEvent += HandlePlayerEvent;
    }
    void OnDisable()
    {
        PlayerEvents.OnPlayerEvent -= HandlePlayerEvent;
    }

    void HandlePlayerEvent(SlugEvents eventType)
    {
        switch (eventType)
        {
            case SlugEvents.HitGround: StartHitGround(); break;
            case SlugEvents.Fall: StartFalling(); break;
            default: break;
        }
    }

    private void StartFalling()
    {
        bottomAnimator.SetTrigger(IsJumpLow);
        topAnimator.SetTrigger(IsJump);
        topAnimator.SetBool(IsJumpLow, true);
    }

    private void StartHitGround()
    {
        topAnimator.SetBool(IsJumpLow, false);
        topAnimator.SetBool(IsJumpHigh, false);
        topAnimator.SetTrigger(IsHitGround);
        bottomAnimator.SetTrigger(IsHitGround);
    }

    public void StartRunningAnimation()
    {
        topAnimator.SetBool(IsRun, true);
        bottomAnimator.SetBool(IsRun, true);
    }

    public void StopRunningAnimation()
    {
        topAnimator.SetBool(IsRun, false);
        bottomAnimator.SetBool(IsRun, false);
    }

    public void StartLookStraightAnimation()
    {
        topAnimator.SetBool(LookDown, false);
        bottomAnimator.SetBool(LookDown, false);
        topAnimator.SetBool(LookUp, false);
    }

    public void StartLookDownAnimation()
    {
        topAnimator.SetBool(LookDown, true);
    }

    public void StartHighVelJumpAnimation()
    {
        bottomAnimator.SetTrigger(IsJumpHigh);
        topAnimator.SetTrigger(IsJump);
        topAnimator.SetBool(IsJumpHigh, true);
    }

    public void StartLowVelJumpAnimation()
    {
        bottomAnimator.SetTrigger(IsJumpLow);
        topAnimator.SetTrigger(IsJump);
        topAnimator.SetBool(IsJumpLow, true);
    }
}
