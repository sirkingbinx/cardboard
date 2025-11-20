using Cardboard.Classes;
using System;

namespace Cardboard.Utils {
    /// <summary>
    /// A manager for a whole lot of events
    /// </summary>
    public static class CardboardEvents {
        /// <summary>
        /// Called when the player joins a room.
        /// </summary>
        public static event Action<RoomEventArgs> OnJoinedRoom = delegate { };

        /// <summary>
        /// Called when the player leaves a room.
        /// </summary>
        public static event Action OnLeftRoom = delegate { };

        /// <summary>
        /// Called when another player joins the current room.
        /// </summary>
        public static event Action<PlayerEventArgs> OnPlayerJoinedRoom = delegate { };

        /// <summary>
        /// Called when another player leaves the current room.
        /// </summary>
        public static event Action<PlayerEventArgs> OnPlayerLeftRoom = delegate { };

        /// <summary>
        /// Called when the master client of the current room is changed.
        /// </summary>
        public static event Action<PlayerEventArgs> OnMasterClientChanged = delegate { };

        /// <summary>
        /// Called when both the game and Cardboard are finished loading.
        /// </summary>
        public static event Action OnPlayerSpawned = delegate { };
    }
}