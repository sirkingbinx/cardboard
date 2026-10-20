using System;
using System.Linq;

namespace Cardboard.Internals;

internal static class CardboardModeProtocol
{
    internal static string ModdedId(string baseMode) => baseMode + ";MODDED_";

    internal static string CustomId(string baseMode, string id)
    {
        if (string.IsNullOrWhiteSpace(id) || id.Any(c =>
            !(c >= 'A' && c <= 'Z') && !(c >= 'a' && c <= 'z') &&
            !(c >= '0' && c <= '9') && c != '_'))
            throw new ArgumentException("Custom mode IDs must contain only ASCII letters, digits or underscores.", nameof(id));

        var moddedId = "MODDED_" + id;
        // Match Utilla's constructor, including IDs that already contain the base name.
        return moddedId.Contains(baseMode) ? moddedId : baseMode + moddedId;
    }

    internal static string RoomMode(string room)
    {
        var fields = room?.Split(';');
        // map;queue;mode;flags;flags -- Utilla's default mode IDs contain a semicolon.
        return fields?.Length >= 5 ? string.Join(";", fields, 2, fields.Length - 4) : null;
    }
}
