using System;
using UnityEngine.Events;

namespace HealthSystem
{
    [Serializable]
    public class HealthEvent : UnityEvent<int, int> { } // (current, max)
}