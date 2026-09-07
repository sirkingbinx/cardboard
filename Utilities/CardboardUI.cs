using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cardboard.Utilities;

/// <summary>
/// Utilities for Unity IMGUI creation.
/// </summary>
public static class CardboardUI
{
    /// <summary>
    /// Creates a dropdown with IMGUI Buttons.
    /// </summary>
    /// <param name="rect">The position and scale of the dropdown.</param>
    /// <param name="current">The currently selected option on the dropdown.</param>
    /// <param name="droppedDown">If the dropdown is expanded or not.</param>
    /// <param name="options">Enumerable list of options on the dropdown.</param>
    /// <param name="toString">Function to convert the type to a string.</param>
    /// <param name="skin">Skin used for the dropdown's button (which holds the selected option)</param>
    /// <param name="optionSkin">Skin used for dropdown options</param>
    /// <typeparam name="T">Type of the IEnumerable.</typeparam>
    /// <returns>Tuple of (T) the currently selected item, and (bool) if the dropdown is down or not.</returns>
    public static (T, bool) Dropdown<T>(Rect rect, T current, bool droppedDown, IEnumerable<T> options, Func<T, string> toString = null!, GUIStyle skin = null!, GUIStyle optionSkin = null!)
    {
        T newCurrent = current;
        bool newDroppedDown = droppedDown;

        if (GUI.Button(rect, toString(current), skin ?? GUI.skin.button))
            newDroppedDown = !newDroppedDown;

        if (!droppedDown)
            return (newCurrent, newDroppedDown);

        foreach (var option in options.Select((item, index) => new { item, index }))
        {
            int newY = (int)(rect.y + (rect.height + (option.index * rect.height)));

            if (GUI.Button(new Rect(rect.x, newY, rect.width, rect.height), toString(option.item), optionSkin ?? GUI.skin.button))
            {
                newCurrent = option.item;
                newDroppedDown = false;
            }
        }

        return (newCurrent, newDroppedDown);
    }
}