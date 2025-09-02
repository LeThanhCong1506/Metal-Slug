using UnityEngine;

public class PlayerExplosives : MonoBehaviour
{
    private static readonly int Attack = Animator.StringToHash("grenade");
    [SerializeField] Transform prefab;
    public void ThrowGrenade(AnimationManager animManager)
    {
        animManager.TopAnimator.SetTrigger(Attack);
        var go = Instantiate(prefab, transform.position, Quaternion.identity);
        go.gameObject.SetActive(true);
    }

}
