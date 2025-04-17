﻿using Photon.Pun;
using Cardboard.Classes;

namespace Cardboard.Internals
{
    public class CardboardNetwork : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            if (NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
                CardboardModded.CallModdedEvent(ModdedEventType.ModdedJoin);
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

            CardboardModded.CallModdedEvent(ModdedEventType.ModdedLeave);
        }
    }
}
