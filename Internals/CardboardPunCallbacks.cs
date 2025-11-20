using Photon.Pun;
using Cardboard.Classes;
using Cardboard.Utils;

namespace Cardboard.Internals
{
    /// <summary>
    /// This is an internal class. Don't use functions from here.
    /// </summary>
    internal class CardboardPunCallbacks : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom()
        {
            var Code = NetworkSystem.Instance.RoomName;
            var IsModded = NetworkSystem.Instance.GameModeString.Contains("MODDED_");
            var IsPrivate = NetworkSystem.Instance.SessionIsPrivate;
            var GamemodeString = NetworkSystem.Instance.GameModeString;
            var Players = NetworkSystem.Instance.AllNetPlayers;
            var MasterClient = NetworkSystem.Instance.MasterClient;

            var RoomEventArgs = new RoomEventArgs(Code, IsModded, IsPrivate, GamemodeString, Players, MasterClient);

            CardboardEvents.OnJoinedRoom(RoomEventArgs);

            if (IsModded)
                CardboardModded.CallModdedEvent(ModdedEventType.ModdedJoin);
        }

        public override void OnLeftRoom()
        {
            CardboardEvents.OnLeftRoom();
            
            if (CardboardModded.IsModded)
                CardboardModded.CallModdedEvent(ModdedEventType.ModdedLeave);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player photonPlayer) =>
            CardboardEvents.OnPlayerJoinedRoom(new PlayerEventArgs(photonPlayer.UserId, photonPlayer.NickName, photonPlayer));

        public override void OnPlayerLeftRoom(Photon.Realtime.Player photonPlayer) =>
            CardboardEvents.OnPlayerLeftRoom(new PlayerEventArgs(photonPlayer.UserId, photonPlayer.NickName, photonPlayer));
    }
}
