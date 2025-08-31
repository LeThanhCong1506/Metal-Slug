using System;

public static class PlayerEvents
{
    public static event Action<SlugGameEvents> OnPlayerEvent;

    public static void Raise(SlugGameEvents eventType)
    {
        OnPlayerEvent?.Invoke(eventType);
    }
}
