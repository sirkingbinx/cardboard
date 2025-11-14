using Valve.VR;
using UnityEngine.XR;

namespace Cardboard.Utils
{
    public class Input
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

        /// <summary>
        /// Indicates if the controller's left primary is being held down.
        /// </summary>
        public static bool leftPrimary
        {
            get => GetValue(InputType.leftPrimary);
        }

        /// <summary>
        /// Indicates if the controller's left secondary is being held down.
        /// </summary>
        public static bool leftSecondary
        {
            get => GetValue(InputType.leftSecondary);
        }

        /// <summary>
        /// Indicates if the controller's left trigger is being pressed in.
        /// </summary>
        public static bool leftTrigger
        {
            get => GetValue(InputType.leftTrigger);
        }

        /// <summary>
        /// Indicates if the controller's left grip is being pressed in.
        /// </summary>
        public static bool leftGrip
        {
            get => GetValue(InputType.leftGrip);
        }

        /// <summary>
        /// Indicates if the controller's left joystick is being held down.
        /// </summary>
        public static bool leftStick
        {
            get => GetValue(InputType.leftStick);
        }

        /// <summary>
        /// Indicates if the controller's right primary is being held down.
        /// </summary>
        public static bool rightPrimary
        {
            get => GetValue(InputType.rightPrimary);
        }

        /// <summary>
        /// Indicates if the controller's right secondary is being held down.
        /// </summary>
        public static bool rightSecondary
        {
            get => GetValue(InputType.rightSecondary);
        }

        /// <summary>
        /// Indicates if the controller's right trigger is being pressed in.
        /// </summary>
        public static bool rightTrigger
        {
            get => GetValue(InputType.rightTrigger);
        }

        /// <summary>
        /// Indicates if the controller's right grip is being pressed in.
        /// </summary>
        public static bool rightGrip
        {
            get => GetValue(InputType.rightGrip);
        }

        /// <summary>
        /// Indicates if the controller's right joystick is being held down.
        /// </summary>
        public static bool rightStick
        {
            get => GetValue(InputType.rightStick);
        }
    }
}