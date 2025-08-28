using UnityEngine;
using Cardboard.Start;
using System.Collections.Generic;
using Cardboard.Classes;

namespace Cardboard.Internals
{
    /// <summary>
    /// This is an internal class. Don't use functions from here.
    /// </summary>
    public class CardboardManager : MonoBehaviour
    {
        public static CardboardManager instance { get; private set; }

        void Start()
        {
            instance = this;

            string platformTag = PlayFabAuthenticator.instance.platform.PlatformTag.ToLower();
            
            if (platformTag.Contains("steam")) {
                Utils.Player.Platform = GamePlatform.Steam
            } else if (platformTag.Contains("oculus")) {
                Utils.Player.Platform = GamePlatform.OculusRift
            } else {
                Utils.Player.Platform = GamePlatform.OculusQuest
            }
        }

        Utils.Player.Steam = Utils.Player.Platform == GamePlatform.Steam
    }
}
