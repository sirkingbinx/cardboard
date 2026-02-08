namespace Cardboard
{
    /// <summary>
    /// Used in Input classes to signify types of controller buttons.
    /// </summary>
    public enum InputType {
        /// <summary>
        /// The left primary button (X)
        /// </summary>
        leftPrimary,

        /// <summary>
        /// The left secondary button (Y)
        /// </summary>
        leftSecondary,

        /// <summary>
        /// The left grip. This is true if the control is pressed in greater than your sensitivity settings (which is 50% by default).
        /// </summary>
        leftGrip,

        /// <summary>
        /// The left trigger. This is true if the control is pressed in greater than your sensitivity settings (which is 50% by default).
        /// </summary>
        leftTrigger,

        /// <summary>
        /// The left joystick click.
        /// </summary>
        leftStick,

        /// <summary>
        /// The right primary button (A)
        /// </summary>
        rightPrimary,

        /// <summary>
        /// The right secondary button (B)
        /// </summary>
        rightSecondary,

        /// <summary>
        /// The right grip. This is true if the control is pressed in greater than your sensitivity settings (which is 50% by default).
        /// </summary>
        rightGrip,

        /// <summary>
        /// The right trigger. This is true if the control is pressed in greater than your sensitivity settings (which is 50% by default).
        /// </summary>
        rightTrigger,

        /// <summary>
        /// The right joystick click.
        /// </summary>
        rightStick,
    };

    /// <summary>
    /// Used in Input classes to signify types of controller buttons.
    /// </summary>
    public enum SpecialInputType {
        /// <summary>
        /// The left thumbstick. Type: Vector2
        /// </summary>
        leftThumbstickAxis,

        /// <summary>
        /// The right thumbstick. Type: Vector2
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
        /// <summary>
        /// Event called as the player joined a modded lobby.
        /// </summary>
        ModdedJoin,

        /// <summary>
        /// Event called as the player left a modded lobby.
        /// </summary>
        ModdedLeave,
    };

    /// <summary>
    /// Used to define the player's platform.
    /// </summary>
    public enum GamePlatform {
        /// <summary>
        /// SteamVR
        /// </summary>
        Steam,

        /// <summary>
        /// Oculus Rift / Link PCVR / Quest Link / etc..
        /// </summary>
        OculusRift,

        /// <summary>
        /// idk bro
        /// </summary>
        None,
    };

    /// <summary>
    /// Used in CardboardLog to declare what type of log message you are delivering.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// A Debug log level, for messages used to help development.
        /// </summary>
        Debug,

        /// <summary>
        /// An Info log level, for messages just meant to give a general message.
        /// </summary>
        Info,

        /// <summary>
        /// A Warning log level, for messages which may cause errors in the future
        /// </summary>
        Warning,

        /// <summary>
        /// An Error log level. Take a guess at what this one means.
        /// </summary>
        Error
    }
}