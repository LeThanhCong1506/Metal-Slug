using UnityEngine;

namespace HealthSystem
{
    public class DamageEffect : MonoBehaviour
    {
        public ParticleSystem hitParticle;   // gán prefab particle
        public SpriteRenderer spriteRenderer;
        public Color flashColor = Color.red;
        public float flashDuration = 0.1f;

        private Color originalColor;

        void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
                originalColor = spriteRenderer.color;
        }

        // Hàm gọi khi Health bị damage
        public void PlayEffect(DamageInfo info)
        {
            // 1. Particle
            if (hitParticle != null)
            {
                hitParticle.Play();
            }

            // 2. Flash màu
            if (spriteRenderer != null)
            {
                StopAllCoroutines();
                StartCoroutine(FlashCoroutine());
            }
        }

        private System.Collections.IEnumerator FlashCoroutine()
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

}