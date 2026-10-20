using System;
using System.Linq;

namespace Cardboard.Internals;

internal static class CardboardModeProtocol
{
    internal static string CustomId(string baseMode, string id)
    {
        if (string.IsNullOrWhiteSpace(id) || id.Any(c =>
            !(c >= 'A' && c <= 'Z') && !(c >= 'a' && c <= 'z') &&
            !(c >= '0' && c <= '9') && c != '_'))
            throw new ArgumentException("Custom mode IDs must contain only ASCII letters, digits or underscores.", nameof(id));

        return baseMode + "MODDED_" + id;
    }

    internal static string RoomMode(string room)
    {
        var fields = room?.Split(';');
        return fields?.Length == 5 ? fields[2] : null;
    }
}
