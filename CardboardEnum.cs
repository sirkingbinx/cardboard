namespace Cardboard
{
    /// <summary>
    /// Used in Input classes to signify types of controller buttons.
    /// </summary>
    public enum InputType {
        leftPrimary,
        leftSecondary,
        leftGrip,
        leftTrigger,
        leftStick,

        rightPrimary,
        rightSecondary,
        rightGrip,
        rightTrigger,
        rightStick,
    };

    /// <summary>
    /// Used to define modded events.
    /// </summary>
    public enum ModdedEventType {
        ModdedJoin,
        ModdedLeave,
    }
}