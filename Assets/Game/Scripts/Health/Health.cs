using UnityEngine;

namespace HealthSystem
{
    public class Health : MonoBehaviour
    {
        [Header("Stats")]
        public int maxHealth = 100;
        [SerializeField]
        private int currentHealth;


        [Header("Invincibility")]
        public bool hasIFrames = true;
        public float iFrameDuration = 0.5f;


        [Header("Death & Events")]
        public bool destroyOnDeath = false; // if true, GameObject.Destroy on death; else invoke death event


        public HealthEvent onHealthChanged; // publish current and max
        public DamageEvent onDamaged; // when damaged (before applying effects)
        public VoidEvent onHealed;
        public VoidEvent onDeath;


        // Internal
        private bool invincible = false;
        private Coroutine iFrameCoroutine;


        void Awake()
        {
            currentHealth = Mathf.Clamp(currentHealth == 0 ? maxHealth : currentHealth, 0, maxHealth);
        }


        void Start()
        {
            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }


        public int Current => currentHealth;
        public int Max => maxHealth;
        public bool IsDead => currentHealth <= 0;


        public void TakeDamage(DamageInfo info)
        {
            if (IsDead) return;
            if (invincible) return;


            // announce before applying, so listeners can sample e.g., hit direction
            onDamaged?.Invoke(info);


            int newHealth = currentHealth - Mathf.Max(0, info.amount);
            currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);

            // start i-frames
        }


        private System.Collections.IEnumerator HandleIFrames(float duration)
        {
            invincible = true;
            // Optional: add visual blink here by toggling renderer or material
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                yield return null;
            }
            invincible = false;
        }


        private void HandleDeath(DamageInfo info)
        {
            onDeath?.Invoke();


            // default behavior: destroy or disable; you can attach listeners to play animation, drop loot, add score
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                // Disable components that control behavior (example): disable collider + scripts
                Collider2D col = GetComponent<Collider2D>();
                if (col != null) col.enabled = false;


                var behaviours = GetComponents<Behaviour>();
                foreach (var b in behaviours)
                {
                    if (b != this) b.enabled = false;
                }
            }
        }


        // Optional external API for instant-kill or revive
        public void Kill()
        {
            TakeDamage(new DamageInfo(currentHealth, Vector2.zero, null, false));
        }


        public void Revive(int healthAmount)
        {
            if (!IsDead) return;
            currentHealth = Mathf.Clamp(healthAmount, 1, maxHealth);
            // re-enable components if you disabled them in HandleDeath (you may want to implement a more robust state manager)
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = true;


            var behaviours = GetComponents<Behaviour>();
            foreach (var b in behaviours)
            {
                if (b != this) b.enabled = true;
            }


            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}