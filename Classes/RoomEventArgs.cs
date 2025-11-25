namespace Cardboard.Classes {
    /// <summary>
    /// Arguments provided by room-based events in Cardboard.Utils.CardboardEvents.
    /// </summary>
    public class RoomEventArgs {
        /// <summary>
        /// The join code of the room.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Represents if the room is modded.
        /// </summary>
        public bool IsModded { get; private set; }

        /// <summary>
        /// Represents if the room is a private room.
        /// </summary>
        public bool IsPrivate { get; private set; }

        /// <summary>
        /// The gamemode string of the room.
        /// </summary>
        public string GamemodeString { get; private set; }

        /// <summary>
        /// An array of all players in the lobby (including the Local Player)
        /// </summary>
        public NetPlayer[] Players { get; private set; }

        /// <summary>
        /// The current master client of the room.
        /// </summary>
        public NetPlayer MasterClient { get; private set; }

        internal RoomEventArgs(string _code, bool _isModded, bool _isPrivate, string _gamemodeString, NetPlayer[] _players, NetPlayer _masterClient) {
            Code = _code;
            IsModded = _isModded;
            IsPrivate = _isPrivate;
            GamemodeString = _gamemodeString;
            Players = _players;
            MasterClient = _masterClient;
        }
    }
}