using Cardboard.Utils;
using Cardboard.Classes;

namespace Cardboard.Interfaces
{
    /// <summary>
    /// Handler for player join/leave events.
    /// </summary>
    public interface ICardboardPlayerHandler {
        /// <summary>
        /// Called when a player joins the current room.
        /// </summary>
        public void OnPlayerJoinedRoom(PlayerEventArgs player) { }

        /// <summary>
        /// Called when a player in the current lobby leaves the room.
        /// </summary>
        public void OnPlayerLeftRoom(PlayerEventArgs player) { }
    }
}