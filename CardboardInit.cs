using BepInEx;
using UnityEngine;
using Cardboard.Utils;

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

    public class CardboardManager : MonoBehaviour
    {
        public static CardboardManager instance;

        public string CardboardVersion = "0.0.0";

        void Start() {
            instance = this;
            
            CardboardVersion = Start.instance.Info.Metadata.Version;
        }
    }
}