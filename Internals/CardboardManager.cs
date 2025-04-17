using UnityEngine;
using Cardboard.Start;

namespace Cardboard.Internals
{
    public class CardboardManager : MonoBehaviour
    {
        public static CardboardManager instance;

        public string CardboardVersion = "0.0.0";

        void Start()
        {
            instance = this;

            CardboardVersion = Bootstrap.instance.Info.Metadata.Version.ToString();
        }
    }
}
