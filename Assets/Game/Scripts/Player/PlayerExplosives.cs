using UnityEngine;

public class PlayerExplosives : MonoBehaviour
{
    private static readonly int Attack = Animator.StringToHash("grenade");

    public void ThrowGrenade(AnimationManager animManager)
    {
        animManager.TopAnimator.SetTrigger(Attack);
        // do action
    }
}
