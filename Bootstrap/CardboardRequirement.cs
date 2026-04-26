using System;

namespace Cardboard.Bootstrap;

/// <summary>
/// CardboardRequirement tells Cardboard that a dependency is required for this mod to function.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CardboardRequirement : Attribute
{
    /// <summary>
    /// The Uuid of the requirement.
    /// </summary>
    public string Uuid;

    /// <inheritdoc cref="CardboardRequirement"/>
    /// <param name="requirementUuid">The Uuid of the requirement.</param>
    /// <param name="necessary">`true` to cancel the mod's initialization if the requirement is missing, `false` to ignore.</param>
    public CardboardRequirement(string requirementUuid, bool necessary = true)
    {

    }
}
