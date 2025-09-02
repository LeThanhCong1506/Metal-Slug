using HealthSystem;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float BulletSpeed;
    [SerializeField] private Rigidbody2D BulletRb;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private int damageAmount = 1;

    private Vector2 BulletDirection;
    private bool isActive = false;
    private bool isDeactivating = false;

    public bool IsActive => isActive;

    void Update()
    {
        if (!isActive) return;
        BulletRb.linearVelocity = BulletDirection.normalized * BulletSpeed;
    }

    void OnBecameInvisible()
    {
        DeActive();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.gameObject.layer == 6) return; // Bỏ qua Player

        // Gây damage
        var health = other.GetComponentInChildren<Health>();
        if (health != null)
        {
            health.TakeDamage(new DamageInfo(damageAmount));
        }

        // Hiệu ứng va chạm
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        DeActive();
    }
    public void Active(Vector2 initPosition, Vector2 newDirection)
    {
        isActive = true;
        isDeactivating = false;

        transform.position = initPosition;
        transform.SetParent(null);

        BulletDirection = newDirection;
        BulletRb.linearVelocity = BulletDirection.normalized * BulletSpeed;

        gameObject.SetActive(true);
    }

    public void DeActive()
    {
        if (isDeactivating) return; 
        isDeactivating = true;

        isActive = false;

        BulletRb.linearVelocity = Vector2.zero;
        BulletDirection = Vector2.zero;

        transform.SetParent(BulletManager.Instance.transform);
        transform.position = Vector3.zero;

        gameObject.SetActive(false);
    }
}
