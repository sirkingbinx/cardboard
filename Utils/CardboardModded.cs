using System;

namespace Cardboard.Utils
{
    /// <summary>
    /// Managed modded room connections.
    /// </summary>
    public static class CardboardModded
    {
        /// <summary>
        /// Designates whether the room the player is currently in is a modded room.
        /// </summary>
        public static bool IsModded
        {
            get => NetworkSystem.Instance.GameModeString.Contains("MODDED");
        }

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
            else if (IsModded && mType == ModdedEventType.ModdedLeave)
                ModdedLeave();
        }
    }
}
