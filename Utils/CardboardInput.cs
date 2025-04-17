using UnityEngine;
using GorillaLocomotion;
using Valve.VR;

namespace Cardboard.Utils
{
    public class Input
    {
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
                    if (Player.Steam)
                        temporarySClick = SteamVR_Actions.gorillaTag_LeftJoystickClick.state;
                    else
                        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out temporarySClick);

                    return temporarySClick;

                // right hand
                case InputType.rightPrimary: return ControllerInputPoller.instance.rightControllerPrimaryButton;
                case InputType.rightSecondary: return ControllerInputPoller.instance.rightControllerSecondaryButton;
                case InputType.rightTrigger: return ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;
                case InputType.rightGrip: return ControllerInputPoller.instance.rightControllerGripFloat > 0.5f;
                case InputType.rightStick:
                    if (Player.Steam)
                        temporarySClick = SteamVR_Actions.gorillaTag_RightJoystickClick.state;
                    else
                        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out temporarySClick);

                    return temporarySClick;
            }

            return false;
        }
    }
}