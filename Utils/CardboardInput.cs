using Valve.VR;
using UnityEngine;
using UnityEngine.XR;

namespace Cardboard.Utils
{
    public static class Input
    {
        /// <summary>
        /// Gets the value of the specified InputType.
        /// </summary>
        /// <param name="_inputType">The InputType of the button you want the value of.</param>
        /// <returns>A bool based on if the button is pressed.</returns>
        public static bool GetValue(InputType _inputType)
        {
            bool temporarySClick = false;

            switch (_inputType)
            {
                // left hand
                case InputType.leftPrimary: return ControllerInputPoller.instance.leftControllerPrimaryButton;
                case InputType.leftSecondary: return ControllerInputPoller.instance.leftControllerSecondaryButton;
                case InputType.leftTrigger: return ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f;
                case InputType.leftGrip: return ControllerInputPoller.instance.leftControllerGripFloat > 0.5f;
                case InputType.leftStick:
                    if (CardboardPlayer.Platform == GamePlatform.Steam)
                        temporarySClick = SteamVR_Actions.gorillaTag_LeftJoystickClick.state;
                    else if (CardboardPlayer.Platform == GamePlatform.OculusRift)
                        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out temporarySClick);

                    return temporarySClick;

                // right hand
                case InputType.rightPrimary: return ControllerInputPoller.instance.rightControllerPrimaryButton;
                case InputType.rightSecondary: return ControllerInputPoller.instance.rightControllerSecondaryButton;
                case InputType.rightTrigger: return ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;
                case InputType.rightGrip: return ControllerInputPoller.instance.rightControllerGripFloat > 0.5f;
                case InputType.rightStick:
                    if (CardboardPlayer.Platform == GamePlatform.Steam)
                        temporarySClick = SteamVR_Actions.gorillaTag_RightJoystickClick.state;
                    else if (CardboardPlayer.Platform == GamePlatform.OculusRift)
                        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out temporarySClick);

                    return temporarySClick;
            }

            return false;
        }

        public static dynamic GetValue(SpecialInputType _inputType) {
            switch (_inputType) {
                case SpecialInputType.leftThumbstickAxis:
                    return ControllerInputPoller.instance.leftPrimary2DAxis;
                case SpecialInputType.rightThumbstickAxis:
                    return ControllerInputPoller.instance.rightPrimary2DAxis;
            }

            return false;
        }

        public static bool leftPrimary => GetValue(InputType.leftPrimary);
        public static bool leftSecondary => GetValue(InputType.leftSecondary);
        public static bool leftTrigger => GetValue(InputType.leftTrigger);
        public static bool leftGrip => GetValue(InputType.leftGrip);
        public static bool leftStick => GetValue(InputType.leftStick);
        public static Vector2 leftAxis => GetValue(SpecialInputType.leftThumbstickAxis);

        public static bool rightPrimary => GetValue(InputType.rightPrimary);
        public static bool rightSecondary => GetValue(InputType.rightSecondary);
        public static bool rightTrigger => GetValue(InputType.rightTrigger);
        public static bool rightGrip => GetValue(InputType.rightGrip);
        public static bool rightStick => GetValue(InputType.rightStick);
        public static Vector2 rightAxis => GetValue(SpecialInputType.rightThumbstickAxis);
    }
}