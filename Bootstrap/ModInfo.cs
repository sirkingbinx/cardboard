using System;

namespace Cardboard.Bootstrap;

/// <summary>
/// Information used by CardboardMod to seperate your mod from others and manage them properly.
/// </summary>

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ModInfo : Attribute
{
    /// <summary>
    /// The display name of the mod.
    /// </summary>
    public string Name;

    /// <summary>
    /// The UUID (unique identifier) of the mod.
    /// </summary>
    public string Uuid;

    /// <summary>
    /// The version of the mod.
    /// </summary>
    public Version Version;

    /// <inheritdoc cref="ModInfo"/>
    /// <param name="name">The display name of the mod.</param>
    /// <param name="uuid">A unique identifier for the mod.</param>
    /// <param name="version">The current version of the mod.</param>
    public ModInfo(string name, string uuid, string version)
    {
        Name = name;
        Uuid = uuid;

        // parse version
        if (!Version.TryParse(version, out Version))
        {
            throw new ArgumentException(message: "The version number provided was invalid. The version must be parsable by `System.Version`.", paramName: "version");
        }
    }
}
