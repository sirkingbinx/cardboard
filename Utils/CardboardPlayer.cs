using System;

namespace Cardboard.Utils
{
    public static class CardboardPlayer
    {
        /// <summary>
        /// Represents the platform the player is using.
        /// </summary>
        public static GamePlatform Platform { get; internal set; }

        /// <summary>
        /// The transform of the user's left hand.
        /// </summary>
        public static Transform LeftHand {
            get => GTPlayer.Instance.LeftHand.controllerTransform;
            set => GTPlayer.Instance.LeftHand.controllerTransform.SetPositionAndRotation(value.position, value.rotation);
        }

        /// <summary>
        /// The transform of the user's right hand.
        /// </summary>
        public static Transform RightHand {
            get => GTPlayer.Instance.RightHand.controllerTransform;
            set => GTPlayer.Instance.RightHand.controllerTransform.SetPositionAndRotation(value.position, value.rotation);
        }
    }
}
