using UnityEngine;
using Cardboard.Utils;
using GorillaNetworking;

namespace Cardboard.Internals
{
    /// <summary>
    /// This is an internal class. Don't use functions from here.
    /// </summary>
    internal class CardboardManager : MonoBehaviour
    {
        internal static CardboardManager instance { get; private set; }

        void Start()
        {
            instance = this;

            string platformTag = PlayFabAuthenticator.instance.platform.PlatformTag.ToLower();
            
            if (platformTag.ToLower().Contains("steam")) {
                CardboardPlayer.Platform = GamePlatform.Steam;
            } else if (platformTag.ToLower().Contains("pc")) {
                CardboardPlayer.Platform = GamePlatform.OculusRift;
            } else {
                CardboardPlayer.Platform = GamePlatform.OculusQuest;
            }

            CardboardConfig.UpdateConfig();
        }
    }
}
