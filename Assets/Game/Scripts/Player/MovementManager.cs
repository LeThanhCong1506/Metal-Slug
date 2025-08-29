using System;
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

public class MovementManager : MonoBehaviour
{
    [SerializeField] private float crouchSpeedFactor = 0.25f;

    private LookDirection lookDirection = LookDirection.Straight;
    private BodyPosture body = BodyPosture.Stand;

    //private TimeUtils timeUtils;
    private PhysicManager physics;
    private AnimationManager animManager;
    private CapsuleCollider2D _collider;

    // Cache default collider values for restoration
    private Vector2 defaultColliderSize;
    private Vector2 defaultColliderOffset;

    void Awake()
    {
        physics = GetComponent<PhysicManager>();
        animManager = GetComponent<AnimationManager>();
        _collider = GetComponent<CapsuleCollider2D>();
        defaultColliderSize = _collider.size;
        defaultColliderOffset = _collider.offset;
    }

    //public void Observe(SlugEvents ev)
    //{
    //    if (ev == SlugEvents.HitGround && lookDirection == LookDirection.Down)
    //    {
    //        //Crouch();
    //    }
    //}

    private void TurnAround(Vector3 dir)
    {
        physics.ChangeDirection(dir);
        if (lookDirection != LookDirection.Up && lookDirection != LookDirection.Down)
        {
            lookDirection = LookDirection.Straight;
        }

    }

    public void HorizontalMovement(Vector3 dir)
    {
        Vector3? turnDir =
    dir == Vector3.right ? Vector3.right :
    dir == Vector3.left ? Vector3.left :
    dir == Vector3.zero && transform.position == Vector3.right ? Vector3.right :
    dir == Vector3.zero && transform.position == Vector3.left ? Vector3.left :
    null;

        if (turnDir.HasValue)
            TurnAround(turnDir.Value);

        if (physics.InTheAir)
        {
            physics.SetVelocityX(dir.x);
        }
        physics.Move(dir.x);
        animManager.StartRunningAnimation();
    }

    public void StopMoving()
    {
        physics.Move(0);
        animManager.StopRunningAnimation();
    }

    public void Jump()
    {
        //if (body == BodyPosture.Crouch)
        //{
        //    LookDown();
        //}
        body = BodyPosture.Stand;
        Debug.Log(Mathf.Abs(physics.GetVelocity().x));
        if (Mathf.Abs(physics.GetVelocity().x) > 0)
        {
            if (physics.JumpHighVel())
            {
                animManager.StartHighVelJumpAnimation();
            }
        }
        else
        {
            if (physics.JumpLowVel())
            {
                animManager.StartLowVelJumpAnimation();
            }
        }
        AdaptColliderStanding();
    }

    //public void LookUp()
    //{
    //    lookDirection = LookDirection.Up;
    //    animManager.StartLookUpAnim();
    //}

    private void LookDown()
    {
        lookDirection = LookDirection.Down;
        animManager.StartLookDownAnimation();
    }

    public void DefaultBodyPosition()
    {
        body = BodyPosture.Stand;
        lookDirection = LookDirection.Straight;
        animManager.StartLookStraightAnimation();
        physics.SetSpeedForGroundMovement();
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
        physics.StopSpeed();
    }

    public void AllowMovement()
    {
        if (body == BodyPosture.Crouch)
        {
            physics.SetSpeedForCrouchMovement();
        }
        else if (body == BodyPosture.Stand)
        {
            physics.SetSpeedForGroundMovement();
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
        _collider.offset = defaultColliderOffset;
        _collider.size = defaultColliderSize;
    }
}
