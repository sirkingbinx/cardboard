using Photon.Pun;
using Cardboard.Classes;
using Cardboard.Utilities;

namespace Cardboard.Internals
{
    /// <summary>
    /// This is an internal class. Don't use functions from here.
    /// </summary>
    internal class CardboardPunCallbacks : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom()
        {
            var code = NetworkSystem.Instance.RoomName;
            var isModded = NetworkSystem.Instance.GameModeString.Contains("MODDED_");
            var isPrivate = NetworkSystem.Instance.SessionIsPrivate;
            var gameModeString = NetworkSystem.Instance.GameModeString;
            var players = NetworkSystem.Instance.AllNetPlayers;
            var masterClient = NetworkSystem.Instance.MasterClient;

            var roomEventArgs = new RoomEventArgs(code, isModded, isPrivate, gameModeString, players, masterClient);

            CardboardEvents.FireJoinedRoom(roomEventArgs);

            if (isModded)
                CardboardModded.CallModdedEvent(ModdedEventType.ModdedJoin);
        }

        public override void OnLeftRoom()
        {
            CardboardEvents.FireLeftRoom();
            
            if (CardboardModded.IsModded)
                CardboardModded.CallModdedEvent(ModdedEventType.ModdedLeave);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player photonPlayer) =>
            CardboardEvents.FirePlayerJoinedRoom(new PlayerEventArgs(photonPlayer.UserId, photonPlayer.NickName, photonPlayer));

        public override void OnPlayerLeftRoom(Photon.Realtime.Player photonPlayer) =>
            CardboardEvents.FirePlayerLeftRoom(new PlayerEventArgs(photonPlayer.UserId, photonPlayer.NickName, photonPlayer));
    }
}
