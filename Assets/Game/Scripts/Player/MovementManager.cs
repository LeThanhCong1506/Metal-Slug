using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Direction the player is looking.
/// </summary>
public enum LookDirection { Straight, Up, Down }

/// <summary>
/// Player's body posture.
/// </summary>
public enum BodyPosture { Stand, Running, InTheAir, Crouch }

public class MovementManager : MonoBehaviour, IObserver
{
    [SerializeField] private float crouchSpeedFactor = 0.25f;

    private LookDirection lookDirection = LookDirection.Straight;
    private BodyPosture body = BodyPosture.Stand;

    //private TimeUtils timeUtils;
    private PhysicManager physics;
    private AnimationManager animManager;
    private List<IObserver> observers;
    private CapsuleCollider2D collider;

    // Cache default collider values for restoration
    private Vector2 defaultColliderSize;
    private Vector2 defaultColliderOffset;

    void Awake()
    {
        physics = GetComponent<PhysicManager>();
        animManager = GetComponent<AnimationManager>();
        //timeUtils = GetComponent<TimeUtils>();
        collider = GetComponent<CapsuleCollider2D>();
        observers = new List<IObserver>(GetComponents<IObserver>());
        defaultColliderSize = collider.size;
        defaultColliderOffset = collider.offset;
    }

    public void Observe(SlugEvents ev)
    {
        if (ev == SlugEvents.HitGround && lookDirection == LookDirection.Down)
        {
            //Crouch();
        }
    }

    private void TurnAround()
    {
        physics.ChangeDirection(-transform.right);
        if (lookDirection == LookDirection.Straight)
        {
            animManager.StartTurnAnimation();
            // Flip look direction
            lookDirection = LookDirection.Straight;
        }
    }

    public void HorizontalMovement(Vector3 dir)
    {
        if (transform.right != dir)
        {
            Debug.Log($"Moving horizontally: {dir}");
            TurnAround();
        }
        if (physics.InTheAir)
        {
            physics.SetVelocityX(dir.x);
        }
        physics.SetForceX(dir.x);
        animManager.StartRunningAnimation();
    }

    public void StopMoving()
    {
        physics.SetForceX(0);
        animManager.StopRunningAnimation();
    }

    //public void Jump()
    //{
    //    if (body == BodyPosture.Crouch)
    //    {
    //        LookDown();
    //    }
    //    body = BodyPosture.Stand;
    //    if (Mathf.Abs(physics.GetVelocityX()) > 0)
    //    {
    //        if (physics.JumpHighVel())
    //        {
    //            timeUtils.FrameDelay(animManager.StartHighVelJumpAnim);
    //        }
    //    }
    //    else
    //    {
    //        if (physics.JumpLowVel())
    //        {
    //            timeUtils.FrameDelay(animManager.StartLowVelJumpAnim);
    //        }
    //    }
    //    AdaptColliderStanding();
    //}

    //public void LookUp()
    //{
    //    lookDirection = LookDirection.Up;
    //    animManager.StartLookUpAnim();
    //}

    //private void LookDown()
    //{
    //    lookDirection = LookDirection.Down;
    //    animManager.StartLookDownAnim();
    //}

    public void DefaultBodyPosition()
    {
        body = BodyPosture.Stand;
        lookDirection = LookDirection.Straight;
        animManager.StartLookStraightAnim();
        physics.SetMovementFactor(physics.groundMovementFactor);
        AdaptColliderStanding();
    }

    //public void DownMovement()
    //{
    //    if (physics.InTheAir)
    //    {
    //        LookDown();
    //    }
    //    else if (body == BodyPosture.Stand)
    //    {
    //        Crouch();
    //    }
    //}

    //private void Crouch()
    //{
    //    body = BodyPosture.Crouch;
    //    lookDirection = LookDirection.Straight;
    //    animManager.StartCrouchAnim();
    //    physics.SetMovementFactor(crouchSpeedFactor);
    //    AdaptColliderCrouching();
    //}

    public void BlockMovement()
    {
        physics.SetMovementFactor(0);
    }

    public void AllowMovement()
    {
        if (body == BodyPosture.Crouch)
        {
            physics.SetMovementFactor(crouchSpeedFactor);
        }
        else if (body == BodyPosture.Stand)
        {
            physics.SetMovementFactor(physics.groundMovementFactor);
        }
    }

    public bool IsInMotion()
    {
        return physics.GetVelocity() != Vector2.zero;
    }

    //private void NotifyObservers(SlugEvents ev)
    //{
    //    foreach (IObserver obs in observers)
    //    {
    //        obs.Observe(ev);
    //    }
    //}

    //private IEnumerator WaitAFrameAndTurnRight()
    //{
    //    yield return new WaitForEndOfFrame();
    //    physics.ChangeDirection(Vector2.right);
    //}

    //private IEnumerator WaitAFrameAndTurnLeft()
    //{
    //    yield return new WaitForEndOfFrame();
    //    physics.ChangeDirection(Vector2.left);
    //}

    //private void AdaptColliderCrouching()
    //{
    //    float newSizeY = defaultColliderSize.y / 2;
    //    float diff = defaultColliderSize.y - newSizeY;
    //    collider.offset = new Vector2(defaultColliderOffset.x, defaultColliderOffset.y - diff / 2);
    //    collider.size = new Vector2(defaultColliderSize.x, newSizeY);
    //}

    private void AdaptColliderStanding()
    {
        collider.offset = defaultColliderOffset;
        collider.size = defaultColliderSize;
    }
}
