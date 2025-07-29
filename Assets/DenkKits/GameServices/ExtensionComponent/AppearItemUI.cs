using UnityEngine;
using DG.Tweening;

namespace DenkKits.GameServices.ExtensionComponent
{
    public class AppearItemUI : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float showDuration = 0.5f;
        [SerializeField] private Ease showEase = Ease.OutBack;
        [SerializeField] private float delay = 0f;

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        private void Start()
        {
            Show();
        }

        private void Show()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;

            transform.DOScale(_originalScale, showDuration)
                .SetEase(showEase)
                .SetDelay(delay);
        }

    }
}