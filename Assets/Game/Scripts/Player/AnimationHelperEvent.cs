using UnityEngine;
using UnityEngine.Events;

public class AnimationHelperEvent : MonoBehaviour
{
    public UnityEvent onStartOfKnifeAnim;

    public void StartOfKnife()
    {
        onStartOfKnifeAnim?.Invoke();
    }
}
