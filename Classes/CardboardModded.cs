using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Cardboard.Utils;

using Photon.Pun;
using GorillaNetworking;
using System.Linq;

namespace Cardboard.Classes
{
    public class CardboardModded
    {
        internal static bool ModdedLobby { get; private set; }

        /// <summary>
        /// Modded join events go here
        /// </summary>
        public static event Action ModdedJoin = delegate { };

        /// <summary>
        /// Modded leave events go here
        /// </summary>
        public static event Action ModdedLeave = delegate { };

        public static void CallModdedEvent(ModdedEventType mType)
        {
            if (mType == ModdedEventType.ModdedJoin)
                ModdedJoin();
            else if (ModdedLobby && mType == ModdedEventType.ModdedLeave)
                ModdedLeave();
        }
    }
}
