using System;
using System.Collections.Generic;

namespace Cardboard;

/// <summary>
/// Extensions used internally by Cardboard.
/// </summary>
public static class CardboardExtensions
{
    /// <summary>
    /// Move an item inside of a list.
    /// </summary>
    /// <typeparam name="T">Type used in the IList.</typeparam>
    /// <param name="list">The list to move items in.</param>
    /// <param name="oldIndex">The index the item is currently at.</param>
    /// <param name="newIndex">The target index of the new item.</param>
    public static void Move<T>(this IList<T> list, int oldIndex, int newIndex)
    {
        if (oldIndex >= list.Count || oldIndex < 0)
            throw new ArgumentOutOfRangeException("oldIndex");

        if (newIndex > list.Count || newIndex < 0)
            throw new ArgumentOutOfRangeException("newIndex");

        T item = list[oldIndex];

        list.RemoveAt(oldIndex);

        if (newIndex < list.Count)
            list.Insert(newIndex, item);
        else
            list.Add(item);
    }
}
