using System;
using UnityEngine;

namespace HealthSystem
{
    [Serializable]
    public struct DamageInfo
    {
        public int amount; // integer damage
        public Vector2 knockback; // optional knockback vector
        public GameObject source; // who caused the damage (bullet, player, etc.)
        public bool isCritical; // for crits


        public DamageInfo(int amount, Vector2 knockback = default, GameObject source = null, bool isCritical = false)
        {
            this.amount = amount;
            this.knockback = knockback;
            this.source = source;
            this.isCritical = isCritical;
        }
    }
}