using System;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static readonly int IsTopRun = Animator.StringToHash("pub");
    private static readonly int IsRun = Animator.StringToHash("horizontal_pressed");

    [SerializeField] private Animator topAnimator;
    [SerializeField] private Animator bottomAnimator;

    public Animator TopAnimator => topAnimator;
    public void StartRunningAnimation()
    {
        topAnimator.SetBool(IsRun, true);
        bottomAnimator.SetBool(IsRun, true);
    }

    public void StopRunningAnimation()
    {
        topAnimator.SetBool("horizontal_pressed", false);
        bottomAnimator.SetBool("horizontal_pressed", false);
    }

    public void StartTurnAnimation()
    {
        topAnimator.SetTrigger("turn");
    }

    public void StartLookStraightAnim()
    {
        topAnimator.SetBool("down_pressed", false);
        bottomAnimator.SetBool("down_pressed", false);
        topAnimator.SetBool("up_pressed", false);
    }
}
