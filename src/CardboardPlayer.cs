using System;
using System.IO;
using GorillaLocomotion;
using UnityEngine;

namespace Cardboard
{
    /// <summary>
    /// A utility class for quick access to player data.
    /// </summary>
    public static class CardboardPlayer
    {
        /// <summary>
        /// Represents the platform the player is using.
        /// </summary>
        public static GamePlatform Platform { get; internal set; }

        /// <summary>
        /// The UserSystem for this player.
        /// </summary>
        public static UserSystem System { get; internal set; }

        /// <summary>
        /// The transform of the user's left hand.
        /// </summary>
        public static Transform LeftHandTransform {
            get => GTPlayer.Instance.LeftHand.controllerTransform;
            set => GTPlayer.Instance.LeftHand.controllerTransform.SetPositionAndRotation(value.position, value.rotation);
        }

        /// <summary>
        /// The transform of the user's right hand.
        /// </summary>
        public static Transform RightHandTransform {
            get => GTPlayer.Instance.RightHand.controllerTransform;
            set => GTPlayer.Instance.RightHand.controllerTransform.SetPositionAndRotation(value.position, value.rotation);
        }
    }
}
