using Photon.Pun;
using Cardboard.Classes;

namespace Cardboard.Internals
{
    /// <summary>
    /// This is an internal class. Don't use functions from here.
    /// </summary>
    internal class CardboardModdedPunCallbacks : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom()
        {
            if (NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
                CardboardModded.CallModdedEvent(ModdedEventType.ModdedJoin);
        }

        public override void OnLeftRoom()
        {
            if (CardboardModded.IsModded)
                CardboardModded.CallModdedEvent(ModdedEventType.ModdedLeave);
        }
    }
}
