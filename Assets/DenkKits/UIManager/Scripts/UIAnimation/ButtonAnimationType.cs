

namespace DenkKits.UIManager.Scripts.UIAnimation
{
    /// <summary> Describes the types of animation available for buttons (and similar components) </summary>
    public enum ButtonAnimationType
    {
        /// <summary>
        /// Punch animation (fast animation that returns to the start values when finished)
        /// </summary>
        Punch,
        
        /// <summary>
        /// State animation (changes the state of the target by setting new values for position, rotation, scale and/or alpha)
        /// </summary>
        State
    }
}