using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PhysicManager : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;

    [Header("Physics Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float maxFallSpeed = -15f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private bool wasGrounded = false;
    private bool inTheAir;
    private bool isGrounded;

    public bool InTheAir => !isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.freezeRotation = true;
        inTheAir = false;
    }

    void Update()
    {
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log("isGrounded: " + isGrounded);
        if (isGrounded && !wasGrounded)
        {
            StopFalling();
        }

        wasGrounded = isGrounded;
    }

    void StopFalling()
    {
        inTheAir = false;
        PlayerEvents.Raise(SlugEvents.HitGround);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        // Set gizmo color based on _isGrounded state
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
#endif

    public void Move(float inputX)
    {
        float targetSpeed = inputX * moveSpeed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);

        if (rb.linearVelocityX > 0)
            transform.localScale = new Vector3(2.5f, 2.5f, 1);
        else if (rb.linearVelocityX < 0)
            transform.localScale = new Vector3(-2.5f, 2.5f, 1);

        //if (inputX != 0)
        //{
        //    ChangeDirection(new Vector3(Mathf.Sign(inputX), 0, 0));
        //}
    }

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

    //public void ChangeDirection(Vector3 newDir)
    //{
    //    if (transform.right != newDir)
    //        transform.right = newDir;
    //}
}
