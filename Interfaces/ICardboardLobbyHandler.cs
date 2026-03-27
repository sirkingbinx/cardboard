using Cardboard.Utils;
using Cardboard.Classes;

namespace Cardboard.Interfaces
{
    /// <summary>
    /// Handler for global lobby join/leave events. This does not do modded checks, use ICardboardModdedHandler for that.
    /// </summary>
    public interface ICardboardLobbyHandler {
        /// <summary>
        /// Called when the current player joins a room.
        /// </summary>
        public void OnJoinedRoom(RoomEventArgs room) { }

        /// <summary>
        /// Called when the current player leaves a room.
        /// </summary>
        public void OnLeftRoom() { }
    }
}