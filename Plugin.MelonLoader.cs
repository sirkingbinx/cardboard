#if MELONLOADER

using Cardboard;
using Cardboard.Internals;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(MLPlugin), Cardboard.Constants.Name, Cardboard.Constants.Version, Cardboard.Constants.Author)]
[assembly: MelonGame("Another Axiom", "Gorilla Tag")]

namespace Cardboard
{
    public class MLPlugin : MelonPlugin
    {
        internal static MLPlugin Instance;

        internal GameObject CardboardManagerGameObject { get; private set; }

        /// <summary>
        /// Don't use this lmao
        /// </summary>
        public override void OnLateInitializeMelon()
        {
            Instance ??= this;
            CardboardManagerGameObject = new GameObject("Cardboard", typeof(CardboardManager));
        }
    }
}

#endif