using BepInEx;
using Cardboard.Internals;
using UnityEngine;

namespace Cardboard
{
    /*
     * When integrating with commonly used mods (like Utilla) we make sure to list their GUID
     * as a soft dependency here.
     *
     * Cardboard should *require* zero dependencies, but for the mods that we do attempt to
     * support, listing them as a soft dependency makes sure that it loads before us.
     */

    /// <summary>
    /// Loads Cardboard into the game. Don't modify this.
    /// </summary>
    [BepInDependency("org.legoandmars.gorillatag.utilla", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class BIEPlugin : BaseUnityPlugin
    {
        internal static BIEPlugin Instance;

        internal GameObject CardboardManagerGameObject { get; private set; }

        private void Start()
        {
            Instance ??= this;
            CardboardManagerGameObject = new GameObject("Cardboard", typeof(CardboardManager));
        }
    }
}
