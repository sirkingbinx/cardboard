using GorillaGameModes;

namespace Cardboard.Interfaces;

/// <summary>
/// Build your own custom gamemodes for Cardboard, Utilla, and/or GorillaLibrary without any additional translation work.
/// </summary>
public abstract class CardboardCustomGamemode
{
    /// <summary>
    /// The ID of the gamemode.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// The display name used for your gamemode.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The gamemode that your custom gamemode inherits behaviour from.
    /// </summary>
    public virtual GameModeType BaseMode { get; } = GameModeType.Casual;

    /// <summary>
    /// Called when a lobby with your gamemode is joined.
    /// </summary>
    /// <param name="roomMode">The gamemode string of the current lobby.</param>
    public virtual void OnJoined(string roomMode) { }

    /// <summary>
    /// Called when a lobby with your gamemode is left.
    /// </summary>
    /// <param name="roomMode">The gamemode string of the lobby that was disconnected from.</param>
    public virtual void OnLeft(string roomMode) { }
}