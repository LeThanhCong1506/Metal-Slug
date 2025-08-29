using Game.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PhysicManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private AnimationManager animManager;
    [SerializeField] private Transform groundCheck;

    [Header("Physics Settings")]
    public float groundDrag = 5f;
    public float airDrag = 0f;
    public float jumpForce = 7f;
    public float maxFallSpeed = -15f;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Debug")]
    public bool debugging = false;

    private bool wasGrounded = false; // Add this field
    private bool inTheAir;
    private bool _isGrounded;

    public bool InTheAir => inTheAir;

    void Awake()
    {
        animManager = GetComponent<AnimationManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.freezeRotation = true;
        inTheAir = false;
    }

    void Update()
    {
        // Limit fall speed (clamp)
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        inTheAir = !_isGrounded;

        // Only call StopFalling() once when landing
        if (_isGrounded && !wasGrounded)
        {
            StopFalling();
        }
        //else if (!_isGrounded && wasGrounded)
        //{
        //    StartFalling();
        //}

        wasGrounded = _isGrounded; // Update for next frame
    }

    void StopFalling()
    {
        inTheAir = false;
        PlayerEvents.Raise(SlugEvents.HitGround);
    }

    void StartFalling()
    {
        inTheAir = true;
        PlayerEvents.Raise(SlugEvents.Fall);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        // Set gizmo color based on _isGrounded state
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
#endif


    // Di chuyển ngang
    public void Move(float inputX)
    {
        float targetSpeed = inputX * moveSpeed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);

        if (inputX != 0)
        {
            ChangeDirection(new Vector3(Mathf.Sign(inputX), 0, 0));
        }
    }

    // Nhảy
    public bool JumpLowVel()
    {
        if (inTheAir) return false;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        inTheAir = true;
        return true;
    }

    public bool JumpHighVel()
    {
        if (inTheAir) return false;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 1.1f);
        inTheAir = true;
        return true;
    }

    // API tiện ích
    public void SetVelocity(Vector2 vel)
    {
        rb.linearVelocity = vel;
    }

    public void SetVelocityX(float velX)
    {
        rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);
    }

    public void SetVelocityY(float velY)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, velY);
        inTheAir = true;
    }

    public void SetSpeedForGroundMovement()
    {
        this.moveSpeed = 3;
    }
    public void SetSpeedForCrouchMovement()
    {
        this.moveSpeed = 1.5f;
    }

    public void StopSpeed()
    {
        this.moveSpeed = 0;
    }

    public Vector2 GetVelocity()
    {
        return rb.linearVelocity;
    }

    public void ChangeDirection(Vector3 newDir)
    {
        if (transform.right != newDir)
            transform.right = newDir;
    }

    private void DebugPrint(string msg)
    {
        if (debugging)
            Debug.Log(msg);
    }
}
