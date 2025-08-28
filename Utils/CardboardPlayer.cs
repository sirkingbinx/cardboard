using HarmonyLib;
using GorillaNetworking;

namespace Cardboard.Utils
{
    public class Player
    {
        [Obsolete("This is deprecated. Please use Platform instead.")]
        public static bool Steam { get; internal set; }

        /// <summary>
        /// Represents the platform the player is using.
        /// </summary>
        public static GamePlatform Platform;
    }
}