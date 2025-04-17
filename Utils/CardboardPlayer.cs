using HarmonyLib;
using GorillaNetworking;

namespace Cardboard.Utils
{
    public class Player
    {
        /// <summary>
        /// Defines whether the player is running on SteamVR.
        /// </summary>
        public static bool Steam { get; internal set; }
    }

    [HarmonyPatch(typeof(GorillaComputer), "Initialise")]
    class GetGamePlatform
    {
        static void Postfix() => Player.Steam = PlayFabAuthenticator.instance.platform.PlatformTag.ToLower().Contains("steam");
    }
}