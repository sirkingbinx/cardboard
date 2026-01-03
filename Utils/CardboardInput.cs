using Valve.VR;
using UnityEngine;
using UnityEngine.XR;
using System;

namespace Cardboard.Utils
{
    /// <summary>
    /// Util class for gathering controller input.
    /// </summary>
    [Obsolete("Please use CardboardInput instead.")]
    public static class Input { };

    /// <summary>
    /// Util class for gathering controller input.
    /// </summary>
    public static class CardboardInput
    {
#region Left Controller
        /// <summary>
        /// Represents if the left controller's primary button is held down.
        /// </summary>
        public static bool LeftPrimary => GetValue(InputType.leftPrimary);

        /// <summary>
        /// Represents if the left controller's secondary button is held down.
        /// </summary>
        public static bool LeftSecondary => GetValue(InputType.leftSecondary);

        /// <summary>
        /// Represents if the left controller's trigger button is pressed in more than 50%.
        /// </summary>
        public static bool LeftTrigger => GetValue(InputType.leftTrigger);

        /// <summary>
        /// Represents if the left controller's grip button is pressed in more than 50%.
        /// </summary>
        public static bool LeftGrip => GetValue(InputType.leftGrip);

        /// <summary>
        /// Represents if the left controller's thumbstick is pressed in.
        /// </summary>
        public static bool LeftStick => GetValue(InputType.leftStick);

        /// <summary>
        /// Represents the position of the left controller's thumbstick.
        /// </summary>
        public static Vector2 LeftAxis => GetValue<Vector2>(SpecialInputType.leftThumbstickAxis);
#endregion
#region Right Controller
        /// <summary>
        /// Represents if the right controller's primary button is held down.
        /// </summary>
        public static bool RightPrimary => GetValue(InputType.rightPrimary);

        /// <summary>
        /// Represents if the right controller's secondary button is held down.
        /// </summary>
        public static bool RightSecondary => GetValue(InputType.rightSecondary);

        /// <summary>
        /// Represents if the right controller's trigger button is pressed in more than 50%.
        /// </summary>
        public static bool RightTrigger => GetValue(InputType.rightTrigger);

        /// <summary>
        /// Represents if the right controller's grip button is pressed in more than 50%.
        /// </summary>
        public static bool RightGrip => GetValue(InputType.rightGrip);

        /// <summary>
        /// Represents if the right controller's thumbstick is pressed in.
        /// </summary>
        public static bool RightStick => GetValue(InputType.rightStick);

        /// <summary>
        /// Represents the position of the right controller's thumbstick.
        /// </summary>
        public static Vector2 RightAxis => GetValue<Vector2>(SpecialInputType.rightThumbstickAxis);
#endregion
#region GetValue
        /// <summary>
        /// Gets the value of the specified InputType.
        /// </summary>
        /// <param name="inputType">The InputType of the button you want the value of.</param>
        /// <returns>A bool based on if the button is pressed.</returns>
        public static bool GetValue(InputType inputType)
        {
            var temporarySClick = false;

            switch (inputType)
            {
                // left hand
                case InputType.leftPrimary: return ControllerInputPoller.instance.leftControllerPrimaryButton;
                case InputType.leftSecondary: return ControllerInputPoller.instance.leftControllerSecondaryButton;
                case InputType.leftTrigger: return ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f;
                case InputType.leftGrip: return ControllerInputPoller.instance.leftControllerGripFloat > 0.5f;
                case InputType.leftStick:
                    switch (CardboardPlayer.Platform)
                    {
                        default:
                        case GamePlatform.None:
                        case GamePlatform.Steam:
                            temporarySClick = SteamVR_Actions.gorillaTag_LeftJoystickClick.state;
                            break;
                        case GamePlatform.OculusRift:
                            ControllerInputPoller.instance.leftControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out temporarySClick);
                            break;
                    }

                    return temporarySClick;

                // right hand
                case InputType.rightPrimary: return ControllerInputPoller.instance.rightControllerPrimaryButton;
                case InputType.rightSecondary: return ControllerInputPoller.instance.rightControllerSecondaryButton;
                case InputType.rightTrigger: return ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;
                case InputType.rightGrip: return ControllerInputPoller.instance.rightControllerGripFloat > 0.5f;
                case InputType.rightStick:
                    switch (CardboardPlayer.Platform)
                    {
                        default:
                        case GamePlatform.None:
                        case GamePlatform.Steam:
                            temporarySClick = SteamVR_Actions.gorillaTag_RightJoystickClick.state;
                            break;
                        case GamePlatform.OculusRift:
                            ControllerInputPoller.instance.rightControllerDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out temporarySClick);
                            break;
                    }

                    return temporarySClick;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Get the value of SpecialInputType. Check the XML documentation of a SpecialInputType to make sure you are using the correct type for the generic.
        /// </summary>
        /// <param name="inputType">The SpecialInputType of the button you want the value of.</param>
        /// <returns>T representing the value.</returns>
        public static T GetValue<T>(SpecialInputType inputType) where T : new() => inputType switch
        {
            SpecialInputType.leftThumbstickAxis =>
                (T)(object)ControllerInputPoller.instance.leftControllerPrimary2DAxis,
            SpecialInputType.rightThumbstickAxis =>
                (T)(object)ControllerInputPoller.instance.rightControllerPrimary2DAxis,

            _ => (T)(object)false
        };

        #endregion
    }
}