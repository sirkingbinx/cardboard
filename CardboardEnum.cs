namespace Cardboard
{
    /// <summary>
    /// Used in Input classes to signify types of controller buttons.
    /// </summary>
#pragma warning disable CS1591
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
    /// Used in Input classes to signify types of controller buttons.
    /// </summary>
    public enum SpecialInputType {
        /// <summary>
        /// Vector2
        /// </summary>
        leftThumbstickAxis,

        /// <summary>
        /// Vector2
        /// </summary>
        rightThumbstickAxis,
    };

    /// <summary>
    /// Used for OS-specific instructions.
    /// </summary>
    public enum SystemEnvironment {
        /// <summary>
        /// Unknown operating system.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Any Linux distribution.
        /// </summary>
        Linux,

        /// <summary>
        /// Windows NT
        /// </summary>
        Windows,

        /// <summary>
        /// Mac OS X, OSX and macOS
        /// </summary>
        Mac,
    };

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
#pragma warning restore CS1591
}