using System;
using System.Reflection;
using Cardboard.Internals;
using Cardboard.Utilities;
using UnityEngine;

namespace Cardboard.Bootstrap;

/// <summary>
/// CardboardMod allows cross-loading of mods across BepInEx and MelonLoader without using loader-specific methods.
/// </summary>
public class CardboardMod : MonoBehaviour
{
    /// <summary>
    /// The log automatically created by Cardboard. If not accessed, no log will be created.
    /// </summary>
    public CardboardLog Logger
    {
        get
        {
            field ??= new CardboardLog(Info.Uuid);
            return field;
        }
    }

    /// <summary>
    /// The ModInfo attached to the CardboardMod.
    /// </summary>
    public ModInfo Info
    {
        get => field;
        set
        {
            if (Assembly.GetCallingAssembly() != typeof(CardboardManager).Assembly)
            {
                throw new AccessViolationException("The Info field may only be updated by Cardboard.");
            }

            field = value;
        }
    }

    /// <summary>
    /// The mod loader that was used to inject Cardboard.
    /// </summary>
    public ModLoader ModLoader
    {
        get => field;
        set
        {
            if (Assembly.GetCallingAssembly() != typeof(CardboardManager).Assembly)
            {
                throw new AccessViolationException("The ModLoader field may only be updated by Cardboard.");
            }

            field = value;
        }
    }

    /// <summary>
    /// Called when Cardboard first initializes the mod.
    /// </summary>
    public void OnBootstrapped() { }
}
