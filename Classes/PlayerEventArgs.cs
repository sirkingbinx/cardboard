namespace Cardboard.Classes {
    /// <summary>
    /// Arguments provided by player-based events in Cardboard.Utils.CardboardEvents
    /// </summary>
    public class PlayerEventArgs {
        /// <summary>
        /// The user ID of the player
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// The current nick name of the player
        /// </summary>
        public string NickName { get; private set; }

        /// <summary>
        /// The NetPlayer of the player
        /// </summary>
        public NetPlayer NetUser { get; private set; }

        /// <summary>
        /// The VRRig of the player (if they are still in the lobby)
        /// </summary>
        public VRRig? PlayerRig { get; private set; }

        /// <summary>
        /// The Photon player of the player (if they are still in the lobby)
        /// </summary>
        public Photon.Realtime.Player? PhotonUser { get; private set; }

        private PlayerEventArgs();
    }
}