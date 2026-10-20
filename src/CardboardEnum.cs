using System;

namespace Cardboard;

/// <summary>
/// Used in Input classes to signify types of controller buttons.
/// </summary>
public enum InputType {
    /// <summary>
    /// The left primary button (X)
    /// </summary>
    LeftPrimary,

    /// <summary>
    /// The left secondary button (Y)
    /// </summary>
    LeftSecondary,

    /// <summary>
    /// The left grip. This is true if the control is pressed in greater than your sensitivity settings (which is 50% by default).
    /// </summary>
    LeftGrip,

    /// <summary>
    /// The left trigger. This is true if the control is pressed in greater than your sensitivity settings (which is 50% by default).
    /// </summary>
    LeftTrigger,

    /// <summary>
    /// The left joystick click.
    /// </summary>
    LeftStick,

    /// <summary>
    /// The right primary button (A)
    /// </summary>
    RightPrimary,

    /// <summary>
    /// The right secondary button (B)
    /// </summary>
    RightSecondary,

    /// <summary>
    /// The right grip. This is true if the control is pressed in greater than your sensitivity settings (which is 50% by default).
    /// </summary>
    RightGrip,

    /// <summary>
    /// The right trigger. This is true if the control is pressed in greater than your sensitivity settings (which is 50% by default).
    /// </summary>
    RightTrigger,

    /// <summary>
    /// The right joystick click.
    /// </summary>
    RightStick,
};

/// <summary>
/// Used in Input classes to signify types of controller buttons.
/// </summary>
public enum SpecialInputType {
    /// <summary>
    /// The left thumbstick. Type: Vector2
    /// </summary>
    LeftThumbstickAxis,

    /// <summary>
    /// The right thumbstick. Type: Vector2
    /// </summary>
    RightThumbstickAxis,
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
    /// Windows NT
    /// </summary>
    Windows,

    /// <summary>
    /// The user is on Linux. This is set when the game is running under Wine or Proton.
    /// </summary>
    Linux
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

/// <summary>
/// The type of channel message.
/// </summary>
public enum ChannelMessageType
{
    /// <summary>
    /// The message was sent to all players in the lobby.
    /// </summary>
    Broadcast = 0,

    /// <summary>
    /// The message was sent to a list of users.
    /// </summary>
    Multicast = 1,

    /// <summary>
    /// The message was sent to a single user.
    /// </summary>
    Unicast = 2
}