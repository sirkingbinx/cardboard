using BepInEx;
using UnityEngine;
using Cardboard.Utils;
using Cardboard.Internals;

namespace Cardboard
{
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