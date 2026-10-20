using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Cardboard.Internals;
using GorillaGameModes;
using GorillaNetworking;
using HarmonyLib;

namespace Cardboard.Patches;

[HarmonyPatch]
internal static class CardboardRoomCreationPatch
{
    private static MethodBase TargetMethod()
    {
        var method = AccessTools.Method(typeof(PhotonNetworkController), "AttemptToJoinSpecificRoomAsync");
        var stateMachine = method.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType
            ?? throw new MissingMemberException("Named-room joining is no longer an async state machine.");
        return AccessTools.Method(stateMachine, "MoveNext");
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var parse = typeof(Enum).GetMethod(nameof(Enum.Parse), new[] { typeof(string), typeof(bool) })
            .MakeGenericMethod(typeof(GameModeType));
        var sizing = AccessTools.Method(AccessTools.TypeByName("RoomSystem"), "GetRoomSizeForCreate");
        int parseCount = 0;
        int sizingCount = 0;

        foreach (var instruction in instructions)
        {
            if (instruction.Calls(parse))
            {
                instruction.opcode = OpCodes.Call;
                instruction.operand = AccessTools.Method(typeof(CardboardRoomCreationPatch), nameof(ParseSelectedMode));
                parseCount++;
            }
            else if (instruction.Calls(sizing))
            {
                instruction.opcode = OpCodes.Call;
                instruction.operand = AccessTools.Method(typeof(CardboardRoomCreationPatch), nameof(GetRoomSize));
                sizingCount++;
            }

            yield return instruction;
        }

        if (parseCount != 1 || sizingCount != 1)
            throw new InvalidOperationException("Named-room creation changed; Cardboard's mode compatibility patch needs review.");
    }

    private static GameModeType ParseSelectedMode(string value, bool ignoreCase)
    {
        var mode = CardboardGamemodes.Find(value);

        if (CardboardGamemodes.Enabled && mode != null)
        {
            // Keep room sizing consistent with the join trigger.
            var trigger = PhotonNetworkController.Instance.currentJoinTrigger;
            return trigger != null ? trigger.GetDesiredGameModeType() : mode.BaseMode;
        }

        return Enum.Parse<GameModeType>(value, ignoreCase);
    }

    private static byte GetRoomSize(GTZone zone, GameModeType mode, bool isPublic, bool subscribed)
    {
        bool customMode = CardboardGamemodes.Enabled &&
            CardboardGamemodes.Find(GorillaComputer.instance.currentGameMode.Value) != null;

        return (byte)AccessTools.Method(AccessTools.TypeByName("RoomSystem"), "GetRoomSizeForCreate").Invoke(
            null, new object[] { zone, mode, customMode ? !isPublic : isPublic, subscribed });
    }
}

[HarmonyPatch]
internal static class CardboardMapRoomSizingPatch
{
    private static MethodBase TargetMethod() =>
        AccessTools.Method(AccessTools.TypeByName("RoomSystem"), "GetVStumpGameModeForSizing");

    private static bool Prefix(ref GameModeType __result)
    {
        if (!CardboardGamemodes.Enabled || GameMode.CurrentGameModeType != GameModeType.None ||
            !GorillaComputer.hasInstance)
            return true;

        var mode = CardboardGamemodes.Find(GorillaComputer.instance.currentGameMode.Value);
        if (mode == null)
            return true;

        __result = mode.BaseMode;
        return false;
    }
}
