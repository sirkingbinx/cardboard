using BepInEx;
using UnityEngine;
using Cardboard.Utils;
using Cardboard.Internals;

namespace Cardboard
{
    /*
     * When integrating with commonly used mods (like Utilla) we make sure to list their GUID
     * as a soft dependency here.
     *
     * Cardboard should *require* zero dependencies, but for the mods that we do attempt to
     * support, listing them as a soft dependency makes sure that it loads after us.
     */
    [BepInDependency("org.legoandmars.gorillatag.utilla", DependencyFlags.SoftDependency)]
    [BepInPlugin("bingus.cardboard", "Cardboard", "1.1.0")]
    public class Main : BaseUnityPlugin
    {
        public static Main instance;
        public static GameObject cardboardObject;

        void Start()
        {
            instance = this;
            cardboardObject = new GameObject("Cardboard", typeof(CardboardManager));
        }
    }
}