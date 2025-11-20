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
        /// The NetPlayer of the player (if they are still in the room)
        /// </summary>
        public NetPlayer? NetUser => NetworkSystem.Instance.AllNetPlayers.First(player => player.UserId == PhotonUser.UserId);

        /// <summary>
        /// The VRRig of the player (if they are still in the room)
        /// </summary>
        public VRRig? Rig => GorillaGameManager.instance.FindPlayerVRRig(NetUser);

        /// <summary>
        /// The Photon realtime player for the user.
        /// </summary>
        public Photon.Realtime.Player PhotonUser { get; private set; }

        private PlayerEventArgs(string _userId, string _nickname, Photon.Realtime.Player _photonUser) {
            UserId = _userId;
            NickName = _nickname;
            PhotonUser = _photonUser;
        };
    }
}
