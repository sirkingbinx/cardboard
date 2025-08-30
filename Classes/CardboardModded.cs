using System;

namespace Cardboard.Classes
{
    public class CardboardModded
    {
        internal static bool IsModded { get; private set; }

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

            IsModded = (mType == ModdedEventType.ModdedJoin);
        }
    }
}
