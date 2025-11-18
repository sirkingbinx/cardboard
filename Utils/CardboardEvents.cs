using Cardboard.Classes;
using System;

namespace Cardboard.Utils {
    /// <summary>
    /// A manager for a whole lot of events
    /// </summary>
    public static class CardboardEvents {
        public static event Action<RoomEventArgs> OnJoinedRoom = delegate { };
        public static event Action<RoomEventArgs> OnLeftRoom = delegate { };

        public static event Action<PlayerEventArgs> OnPlayerJoinedRoom = delegate { };
        public static event Action<PlayerEventArgs> OnPlayerLeftRoom = delegate { };
        public static event Action<PlayerEventArgs> OnMasterClientChanged = delegate { };
    }
}