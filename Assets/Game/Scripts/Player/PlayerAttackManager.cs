using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField]
    private MeleeAttack meleeAttack;

    public void Attack()
    {
        meleeAttack.AttackType1();
    }
}
