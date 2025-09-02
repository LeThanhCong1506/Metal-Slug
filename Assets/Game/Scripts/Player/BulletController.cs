using HealthSystem;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float BulletSpeed;
    [SerializeField]
    private Vector2 BulletDirection;
    [SerializeField]
    private Rigidbody2D BulletRb;
    [SerializeField]
    private GameObject impactEffect;
    [SerializeField]
    private int damageAmount = 1;

    private bool isActive = false;
    public bool IsActive => isActive;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        BulletRb.linearVelocity = BulletDirection * BulletSpeed;
    }

    void OnBecameInvisible()
    {
        DeActive();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obtacle = collision.gameObject.GetComponent<Health>();
        if (obtacle != null)
        {
            Debug.Log("DoAttack");
            obtacle.TakeDamage(new DamageInfo(50));
        }
        //if (collision.CompareTag("Enemy"))
        //{
        //    collision.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);

        //    EnemyHealthController enemyHealth = collision.GetComponent<EnemyHealthController>();
        //    if (enemyHealth != null)
        //    {
        //        enemyHealth.DamageEnemy(damageAmount);
        //    }
        //}

        //if (collision.CompareTag("Boss"))
        //{
        //    BossHealthController bossHealth = collision.GetComponentInParent<BossHealthController>();
        //    if (bossHealth != null)
        //    {
        //        bossHealth.TakeDamage(damageAmount);
        //    }
        //}

        Instantiate(impactEffect, transform.position, Quaternion.identity);
        DeActive();
    }

    public void Active(Vector2 initPosition, Vector2 newDirection)
    {
        isActive = true;
        this.gameObject.SetActive(true);
        this.transform.position = initPosition;
        this.transform.SetParent(null);
        BulletDirection = newDirection;
    }

    public void DeActive()
    {
        isActive = false;
        this.transform.position = Vector3.zero;
        this.gameObject.SetActive(false);
        BulletRb.linearVelocity = Vector2.zero;
        BulletDirection = Vector2.zero;
        this.transform.SetParent(BulletManager.Instance.transform);
    }
}
