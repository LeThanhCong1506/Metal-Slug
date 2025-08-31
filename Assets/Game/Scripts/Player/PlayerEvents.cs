using System;

public static class PlayerEvents
{
    public static event Action<SlugEvents> OnPlayerEvent;

    public static void Raise(SlugEvents eventType)
    {
        OnPlayerEvent?.Invoke(eventType);
    }
}
