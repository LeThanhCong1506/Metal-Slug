using DG.Tweening;
using UnityEngine;

public class FloatingItemUI : MonoBehaviour
{
    [Header("Floating Config")]
    [SerializeField] private float floatStrength = 10f; // Độ cao dao động (pixel)
    [SerializeField] private float floatDuration = 1.5f; // Thời gian lên/xuống
    [SerializeField] private Ease floatEase = Ease.InOutSine;

    private Vector3 initialPos;
    private Tween floatTween;

    private void Start()
    {
        initialPos = transform.localPosition;
        StartFloating();
    }

    private void StartFloating()
    {
        floatTween = transform.DOLocalMoveY(initialPos.y + floatStrength, floatDuration)
            .SetEase(floatEase)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        floatTween?.Kill();
    }

    private void OnDestroy()
    {
        floatTween?.Kill();
    }
}
