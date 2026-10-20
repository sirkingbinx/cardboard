using System;

namespace Cardboard;

/// <summary>
/// Utilities for working with the user's system.
/// </summary>
public static class CardboardSystem
{
    /// <inheritdoc cref="UserSystem.GetFolderPath"/>
    public static string GetFolderPath(
        Environment.SpecialFolder folder,
        Environment.SpecialFolderOption option = Environment.SpecialFolderOption.None
    ) => CardboardPlayer.System.GetFolderPath(folder, option);

    /// <inheritdoc cref="UserSystem.SystemEnvironment"/>
    public static SystemEnvironment SystemEnvironment => CardboardPlayer.System.SystemEnvironment;
}