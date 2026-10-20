using UnityEngine;
using UnityEngine.XR;

namespace Cardboard
{
    /// <summary>
    /// Util class for gathering controller input.
    /// </summary>
    public static class CardboardInput
    {
        #region Left Controller
        /// <summary>
        /// Represents if the left controller's primary button is held down.
        /// </summary>
        public static bool LeftPrimary => GetValue(InputType.LeftPrimary);

        /// <summary>
        /// Represents if the left controller's secondary button is held down.
        /// </summary>
        public static bool LeftSecondary => GetValue(InputType.LeftSecondary);

        /// <summary>
        /// Represents if the left controller's trigger button is pressed in more than 50%.
        /// </summary>
        public static bool LeftTrigger => GetValue(InputType.LeftTrigger);

        /// <summary>
        /// Represents if the left controller's grip button is pressed in more than 50%.
        /// </summary>
        public static bool LeftGrip => GetValue(InputType.LeftGrip);

        /// <summary>
        /// Represents if the left controller's thumbstick is pressed in.
        /// </summary>
        public static bool LeftStick => GetValue(InputType.LeftStick);

        /// <summary>
        /// Represents the position of the left controller's thumbstick.
        /// </summary>
        public static Vector2 LeftAxis => ControllerInputPoller.instance.leftControllerPrimary2DAxis;
#endregion
        #region Right Controller
        /// <summary>
        /// Represents if the right controller's primary button is held down.
        /// </summary>
        public static bool RightPrimary => GetValue(InputType.RightPrimary);

        /// <summary>
        /// Represents if the right controller's secondary button is held down.
        /// </summary>
        public static bool RightSecondary => GetValue(InputType.RightSecondary);

        /// <summary>
        /// Represents if the right controller's trigger button is pressed in more than 50%.
        /// </summary>
        public static bool RightTrigger => GetValue(InputType.RightTrigger);

        /// <summary>
        /// Represents if the right controller's grip button is pressed in more than 50%.
        /// </summary>
        public static bool RightGrip => GetValue(InputType.RightGrip);

        /// <summary>
        /// Represents if the right controller's thumbstick is pressed in.
        /// </summary>
        public static bool RightStick => GetValue(InputType.RightStick);

        /// <summary>
        /// Represents the position of the right controller's thumbstick.
        /// </summary>
        public static Vector2 RightAxis => ControllerInputPoller.instance.rightControllerPrimary2DAxis;
        #endregion
        
        #region GetValue
        /// <summary>
        /// Gets the value of the specified InputType.
        /// </summary>
        /// <param name="inputType">The InputType of the button you want the value of.</param>
        /// <param name="sensitivity">The sensitivity to check for inputs that are not direct boolean values (such as triggers or grips).</param>
        /// <returns>A bool based on if the button is pressed.</returns>
        public static bool GetValue(InputType inputType, float sensitivity = 0.5f)
        {
            var temporarySClick = false;

            switch (inputType)
            {
                // left hand
                case InputType.LeftPrimary: return ControllerInputPoller.instance.leftControllerPrimaryButton;
                case InputType.LeftSecondary: return ControllerInputPoller.instance.leftControllerSecondaryButton;
                case InputType.LeftTrigger: return ControllerInputPoller.instance.leftControllerIndexFloat > sensitivity;
                case InputType.LeftGrip: return ControllerInputPoller.instance.leftControllerGripFloat > sensitivity;
                case InputType.LeftStick:
                    ControllerInputPoller.instance.leftControllerDevice.TryGetFeatureValue(
                        CommonUsages.primary2DAxisClick,
                        out temporarySClick);
                    return temporarySClick;

                // right hand
                case InputType.RightPrimary: return ControllerInputPoller.instance.rightControllerPrimaryButton;
                case InputType.RightSecondary: return ControllerInputPoller.instance.rightControllerSecondaryButton;
                case InputType.RightTrigger: return ControllerInputPoller.instance.rightControllerIndexFloat > sensitivity;
                case InputType.RightGrip: return ControllerInputPoller.instance.rightControllerGripFloat > sensitivity;
                case InputType.RightStick:
                    ControllerInputPoller.instance.rightControllerDevice.TryGetFeatureValue(
                        CommonUsages.primary2DAxisClick,
                        out temporarySClick);
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
            SpecialInputType.LeftThumbstickAxis =>
                (T)(object)ControllerInputPoller.instance.leftControllerPrimary2DAxis,
            SpecialInputType.RightThumbstickAxis =>
                (T)(object)ControllerInputPoller.instance.rightControllerPrimary2DAxis,

            _ => (T)(object)false
        };

        #endregion
    }
}