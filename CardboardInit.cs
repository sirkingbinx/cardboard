using BepInEx;
using UnityEngine;
using Cardboard.Utils;
using Cardboard.Internals;

namespace Cardboard.Start
{
    [BepInPlugin("kingbingus.cardboard", "Cardboard", "1.0.0")]
    public class Bootstrap : BaseUnityPlugin
    {
        public static Bootstrap instance;

        void Start()
        {
            instance = this;
            CardboardHarmony.PatchInstance(this);
            new GameObject("CardboardManager", typeof(CardboardManager));
        }
    }
}