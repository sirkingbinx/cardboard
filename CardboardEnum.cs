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

    public enum SpecialInputType {
        leftThumbstickAxis,
        rightThumbstickAxis,
    }

    /// <summary>
    /// Used to define modded events.
    /// </summary>
    public enum ModdedEventType {
        ModdedJoin,
        ModdedLeave,
    };

    /// <summary>
    /// Used to define the player's platform.
    /// </summary>
    public enum GamePlatform {
        Steam,
        OculusRift,
        None,
    };
}