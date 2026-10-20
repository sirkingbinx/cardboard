using System;
using Cardboard.Internals;

namespace Cardboard
{
    /// <summary>
    /// Managed modded room connections.
    /// </summary>
    public static class CardboardModded
    {
        /// <summary>
        /// Designates whether the room the player is currently in is a modded room.
        /// </summary>
        public static bool IsModded => NetworkSystem.Instance != null && NetworkSystem.Instance.InRoom &&
            (NetworkSystem.Instance.GameModeString?.Contains("MODDED_") ?? false);

        /// <summary>
        /// Modded join events go here
        /// </summary>
        public static event Action ModdedJoin = delegate { };

        /// <summary>
        /// Modded leave events go here
        /// </summary>
        public static event Action ModdedLeave = delegate { };

        internal static void CallModdedEvent(ModdedEventType mType)
        {
            if (mType == ModdedEventType.ModdedJoin)
                ModdedJoin();
            else if (mType == ModdedEventType.ModdedLeave)
                ModdedLeave();

        }
    }
}
