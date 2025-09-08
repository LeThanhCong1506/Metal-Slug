using UnityEngine;
using HealthSystem;

public class Grenade : MonoBehaviour
{
    [Header("Grenade Settings")]
    public float throwForce = 10f;
    public float upwardForce = 5f;
    public float gravity = -9.8f;   // trọng lực tự code

    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    public float explosionForce = 700f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [Header("References")]
    public GameObject explosionEffect;
    public GameObject owner;

    private bool wasGrounded = false;
    private bool isGrounded;
    private Vector2 velocity;
    private bool hasExploded = false;

 
    void OnEnable()
    {
        hasExploded = false;

        int isRight = owner.transform.localScale.x >= 0 ? 1 : -1;
        Vector2 throwDir = new Vector2(isRight, 0);

        velocity = throwDir * throwForce + Vector2.up * upwardForce;
    }
    void Update()
    {
        if (hasExploded) return;

        velocity.y += gravity * Time.deltaTime;

        transform.position += (Vector3)(velocity * Time.deltaTime);

        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
        Debug.Log("isGrounded: " + isGrounded);
        if (isGrounded && !wasGrounded)
        {
            Explode();
        }

        wasGrounded = isGrounded;
    }
    void Explode()
    {
        hasExploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D nearby in colliders)
        {
            if (owner != null && nearby.gameObject == owner) continue;

            Rigidbody2D body = nearby.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                Vector2 dir = (body.transform.position - transform.position).normalized;
                body.AddForce(dir * explosionForce);
            }

            var health = nearby.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(new DamageInfo((int)explosionDamage));
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
