using System;
using UnityEngine.Events;

namespace HealthSystem
{
    [Serializable]
    public class DamageEvent : UnityEvent<DamageInfo> { }
}