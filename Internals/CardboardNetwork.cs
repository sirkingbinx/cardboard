using Photon.Pun;
using Cardboard.Classes;

namespace Cardboard.Internals
{
    /// <summary>
    /// This is an internal class. Don't use functions from here.
    /// </summary>
    public class CardboardNetwork : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom() => NetworkSystem.Instance.GameModeString.Contains("MODDED_") ? CardboardModded.CallModdedEvent(ModdedEventType.ModdedJoin);
        public override void OnLeftRoom() => CardboardModded.CallModdedEvent(ModdedEventType.ModdedLeave);
    }
}
