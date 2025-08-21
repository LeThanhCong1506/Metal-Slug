using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicManager : MonoBehaviour
{
    private const float RayCastRestLength = 0.03f;
    private const float MaxSlope = 0.8f;
    private const float SlopeSlideFactor = 1.8f;
    private const float YTranslationOffset = 0.005f;
    private const float XTranslationOffset = 0.03f;

    private Collider2D _collider;
    private List<IObserver> observers;

    [Header("Physics Settings")]
    public float groundDrag = 0.8f;
    public float airDrag = 0.998f;
    public float initialJumpVelocity = 5f;
    public float maxVerticalVelocity = -10f;
    public float verticalDrag = 10f;
    public float bounceFactor = 0f;

    [Header("Movement Factors")]
    public float groundMovementFactor = 1.2f;
    public float airLowVelocityMovementFactor = 1.0f;
    public float airHighVelocityMovementFactor = 1.5f;

    [Header("Debug")]
    public bool debugging = false;
    public LayerMask linecastLayerMask;

    private float movementFactor;
    private Vector2 velocity;
    private RaycastHit2D[] rayCastHit = new RaycastHit2D[1];
    private bool inTheAir = false;
    public bool InTheAir { get { return inTheAir; } }
    private float forceX;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        observers = new List<IObserver>(GetComponents<IObserver>());
        velocity = Vector2.zero;
        movementFactor = groundMovementFactor;
    }

    void Update()
    {
        CalculateVelocity();

        Vector2 groundSlope = WhatIsUnderMyFeet(Vector2.zero) > 0
            ? GetSlopeFromRayCastHit2D(rayCastHit[0])
            : Vector2.zero;

        Vector2 transCandidate = CalculateTranslation(groundSlope);

        // Adjust X if collision ahead
        if (WhatIsInFrontOfMe(transCandidate) > 0)
        {
            Vector2 facingWallSlope = GetSlopeFromRayCastHit2D(rayCastHit[0]);
            if (Mathf.Abs(facingWallSlope.y) > MaxSlope)
            {
                transCandidate.x = FixXTrans(rayCastHit[0]);
                if (!inTheAir)
                {
                    transCandidate = Vector2.zero;
                }
            }
        }

        // Adjust Y if collision below
        if (WhatIsUnderMyFeet(transCandidate) > 0)
        {
            Vector2 futureUnderSlope = GetSlopeFromRayCastHit2D(rayCastHit[0]);
            if (inTheAir && velocity.y < 0)
            {
                if (Mathf.Abs(futureUnderSlope.y) < MaxSlope)
                {
                    StopFalling();
                }
                transCandidate.y = FixYTrans(rayCastHit[0]);
            }
            else if (!inTheAir)
            {
                transCandidate.y = FixYTrans(rayCastHit[0]);
            }
        }
        else if (!inTheAir)
        {
            StartFalling();
        }

        transform.Translate(transCandidate.x, transCandidate.y, 0, Space.World);
    }

    private Vector2 CalculateTranslation(Vector2 groundSlope)
    {
        Vector2 trans = Vector2.zero;
        if (inTheAir && groundSlope != Vector2.zero && velocity.y < 0)
        {
            // Sliding on a steep slope
            trans.x = SlopeSlideFactor * Time.deltaTime * Mathf.Abs(groundSlope.x);
            trans.y = -SlopeSlideFactor * Time.deltaTime * Mathf.Abs(groundSlope.y);
        }
        else if (inTheAir)
        {
            // Falling or ascending
            trans.x = velocity.x * movementFactor * Time.deltaTime;
            trans.y = velocity.y * Time.deltaTime;
        }
        else
        {
            // On the ground
            trans.x = velocity.x * movementFactor * Time.deltaTime * Mathf.Abs(groundSlope.x);
            trans.y = Mathf.Abs(velocity.x) * movementFactor * Time.deltaTime * groundSlope.y;
        }
        DebugPrint($"{trans.x} {trans.y}");
        return trans;
    }

    private int WhatIsUnderMyFeet(Vector2 trans)
    {
        Vector2 endPoint = new Vector2(_collider.bounds.center.x + trans.x, _collider.bounds.min.y + trans.y - RayCastRestLength);
        Vector2 startPoint = new Vector2(endPoint.x, _collider.bounds.min.y + RayCastRestLength);
        int hitCount = Physics2D.Linecast(startPoint, endPoint, new ContactFilter2D { layerMask = linecastLayerMask, useLayerMask = true }, rayCastHit);
        return hitCount;
    }

    private int WhatIsInFrontOfMe(Vector2 trans)
    {
        Bounds bounds = _collider.bounds;
        float startX = bounds.center.x;

        Vector2 startPoint = new Vector2(startX, bounds.min.y);
        Vector2 endPoint = startPoint + new Vector2(trans.x, trans.y);

        int hitCount = Physics2D.Linecast(startPoint, endPoint, new ContactFilter2D { layerMask = linecastLayerMask, useLayerMask = true }, rayCastHit);
        Debug.DrawLine(startPoint, endPoint);
        if (hitCount == 0)
        {
            startPoint = new Vector2(startX, bounds.max.y);
            endPoint = startPoint + new Vector2(trans.x, trans.y);
            hitCount = Physics2D.Linecast(startPoint, endPoint, new ContactFilter2D { layerMask = linecastLayerMask, useLayerMask = true }, rayCastHit);
        }
        return hitCount;
    }

    private float FixXTrans(RaycastHit2D hit)
    {
        if (transform.right == Vector3.left)
        {
            return rayCastHit[0].point.x - _collider.bounds.center.x + XTranslationOffset;
        }
        else
        {
            return rayCastHit[0].point.x - _collider.bounds.center.x - XTranslationOffset;
        }
    }

    private float FixYTrans(RaycastHit2D hit)
    {
        return hit.point.y - _collider.bounds.min.y + YTranslationOffset;
    }

    private Vector2 GetSlopeFromRayCastHit2D(RaycastHit2D hit)
    {
        Quaternion rotate = Quaternion.Euler(0, 0, -90 * transform.right.x);
        return rotate * hit.normal;
    }

    private void StopFalling()
    {
        inTheAir = false;
        movementFactor = groundMovementFactor;
        velocity.y = 0;
        NotifyObservers(SlugEvents.HitGround);
    }

    private void StartFalling()
    {
        inTheAir = true;
        movementFactor = airLowVelocityMovementFactor;
        NotifyObservers(SlugEvents.Fall);
    }

    private void CalculateVelocity()
    {
        if (inTheAir)
        {
            velocity.x *= airDrag;
            DebugPrint("pre calculate " + velocity.y);
            velocity.y -= (verticalDrag * Time.deltaTime);
            DebugPrint($"calculate {velocity.y} {Time.deltaTime} {verticalDrag}");
        }
        else
        {
            velocity.x = velocity.x * groundDrag + forceX;
        }
    }

    /// <summary>
    /// Initiates a low velocity jump.
    /// </summary>
    public bool JumpLowVel()
    {
        if (inTheAir) return false;
        inTheAir = true;
        movementFactor = airLowVelocityMovementFactor;
        velocity.y = initialJumpVelocity;
        return true;
    }

    /// <summary>
    /// Initiates a high velocity jump.
    /// </summary>
    public bool JumpHighVel()
    {
        if (inTheAir) return false;
        inTheAir = true;
        movementFactor = airHighVelocityMovementFactor;
        velocity.y = initialJumpVelocity;
        return true;
    }

    /// <summary>
    /// Sets both X and Y velocity.
    /// </summary>
    public void SetVelocity(float velX, float velY)
    {
        velocity.x = velX * transform.right.x;
        velocity.y = velY * transform.up.y;
    }

    /// <summary>
    /// Sets X velocity.
    /// </summary>
    public void SetVelocityX(float velX)
    {
        velocity.x = velX;
    }

    /// <summary>
    /// Sets Y velocity and marks as airborne.
    /// </summary>
    public void SetVelocityY(float velY)
    {
        velocity.y = velY;
        inTheAir = true;
    }

    public float GetVelocityX()
    {
        return velocity.x;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

    public void SetForceX(float forceX)
    {
        this.forceX = forceX;
    }

    public void ChangeDirection(Vector3 newDir)
    {
        if (transform.right != newDir)
        {
            transform.right = newDir;
        }
    }

    public void MoveForward(float vel = 1f)
    {
        velocity.x = transform.right.x * vel;
    }

    public void SetMovementFactor(float movementFactor)
    {
        this.movementFactor = movementFactor;
    }

    private void NotifyObservers(SlugEvents ev)
    {
        if (observers == null) return;
        foreach (IObserver obs in observers)
        {
            obs.Observe(ev);
        }
    }

    private void DebugPrint(string message)
    {
        if (debugging)
        {
            Debug.Log(message);
        }
    }
}
