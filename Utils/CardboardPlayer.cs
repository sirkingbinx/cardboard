using HarmonyLib;
using GorillaNetworking;

namespace Cardboard.Utils
{
    public class CardboardPlayer
    {
        [Obsolete("This is deprecated. Please use Platform instead.")]
        public static bool Steam { get; internal set; }

        /// <summary>
        /// Represents the platform the player is using.
        /// </summary>
        public static GamePlatform Platform;
    }

    [Obsolete("This is deprecated. Please use CardboardPlayer instead.")]
    public class Player {
        [Obsolete("This is deprecated. Please use CardboardPlayer.Platform instead.")]
        public static bool Steam { get; internal set; }
    }
}
