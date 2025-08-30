using System;
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
    private static readonly int LookUpAnim = Animator.StringToHash("look_up_trigger");
    private static readonly int Fire = Animator.StringToHash("fire");
    private static readonly int Knifeing = Animator.StringToHash("knifeing");

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
            case SlugEvents.HitGround: 
                StartHitGround();
                StartLookStraightAnimation();
                break;
            case SlugEvents.Fall: 
                StartFalling(); 
                break;
            default: 
                break;
        }
    }

    public void StartShootingAnimation()
    {
        topAnimator.SetTrigger(Fire);
    }

    public void StartShootingDownAnimation()
    {
        topAnimator.SetTrigger(Fire);
        topAnimator.SetBool(IsJumpLow, true);
        topAnimator.SetBool(LookDown, true);
        topAnimator.SetBool(Knifeing, true);
    }

    public void StartShootingUpAnimation()
    {
        topAnimator.SetBool(LookUp, true);
        topAnimator.SetTrigger(Fire);
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

    public void StartLookUpAnimation()
    {
        if (!topAnimator.GetBool(LookUp))
        {
            if (!topAnimator.GetBool(IsJumpLow)
                    && !topAnimator.GetBool(IsJumpHigh)
                    && !topAnimator.GetBool(LookDown))
            {
                topAnimator.SetTrigger(LookUpAnim);
            }
        }
        topAnimator.SetBool(LookUp, true);
    }
}
