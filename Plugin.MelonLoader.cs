using Cardboard;
using Cardboard.Internals;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(PluginMelonLoader), Cardboard.Constants.Name, Cardboard.Constants.Version, Cardboard.Constants.Author)]
[assembly: MelonGame("Another Axiom", "Gorilla Tag")]
[assembly: HarmonyDontPatchAll]

namespace Cardboard;

internal class PluginMelonLoader : MelonPlugin
{
    internal static PluginMelonLoader Instance;

    internal GameObject CardboardManagerGameObject { get; private set; }

    /// <summary>
    /// Don't use this lmao
    /// </summary>
    public override void OnLateInitializeMelon()
    {
        Instance ??= this;
        Constants.Loader = ModLoader.MelonLoader;
        CardboardManagerGameObject = new GameObject("Cardboard", typeof(CardboardManager));
    }
}