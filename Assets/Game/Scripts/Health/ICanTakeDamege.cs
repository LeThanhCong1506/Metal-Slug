using UnityEngine;

namespace HealthSystem
{
    public interface ICanTakeDamege
    {
        void TakeDamage(int v);
        void TakeDamage(int damage, GameObject source);
    }
}