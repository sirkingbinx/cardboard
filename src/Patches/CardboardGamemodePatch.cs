using System;
using Cardboard.Internals;
using GorillaGameModes;
using GorillaNetworking;
using GorillaTagScripts.VirtualStumpCustomMaps;
using HarmonyLib;
using UnityEngine;

namespace Cardboard.Patches;

[HarmonyPatch(typeof(GameModeSelectorButtonLayout), "SetupButtons")]
internal static class CardboardSelectorPatch
{
    private static bool Prefix(GameModeSelectorButtonLayout __instance)
    {
        if (!CardboardGamemodes.Enabled)
            return true;

        CardboardGamemodes.Attach(__instance);
        return false;
    }
}

[HarmonyPatch(typeof(CustomMapModeSelector), "SetupButtons")]
internal static class CardboardMapSelectorPatch
{
    private static bool Prefix(GameModeSelectorButtonLayout __instance)
    {
        if (!CardboardGamemodes.Enabled)
            return true;

        CardboardGamemodes.Attach(__instance);
        return false;
    }
}

[HarmonyPatch(typeof(GorillaNetworkJoinTrigger), nameof(GorillaNetworkJoinTrigger.GetDesiredGameModeType))]
internal static class CardboardDesiredModeTypePatch
{
    private static bool Prefix(GorillaNetworkJoinTrigger __instance, ref GameModeType __result)
    {
        var mode = CardboardGamemodes.Find(GorillaComputer.instance.currentGameMode.Value);
        if (!CardboardGamemodes.Enabled || __instance.zone == GTZone.ranked || __instance is GorillaNetworkRankedJoinTrigger)
            return true;

        GameModeType baseMode;
        if (mode != null)
            baseMode = mode.BaseMode;
        else if (!Enum.TryParse(GorillaComputer.instance.currentGameMode.Value, out baseMode) ||
            (baseMode != GameModeType.SuperCasual && baseMode != GameModeType.SuperInfect))
            return true;

        var zone = __instance.zone == GTZone.none ? VRRig.LocalRig.zoneEntity.currentZone : __instance.zone;
        __result = CardboardGamemodes.Verify(zone, baseMode);
        return false;
    }
}

[HarmonyPatch(typeof(GorillaNetworkJoinTrigger), nameof(GorillaNetworkJoinTrigger.GetDesiredGameType))]
internal static class CardboardDesiredModePatch
{
    private static void Postfix(GorillaNetworkJoinTrigger __instance, ref string __result)
    {
        if (!CardboardGamemodes.Enabled || __instance.zone == GTZone.ranked || __instance is GorillaNetworkRankedJoinTrigger)
            return;

        var mode = CardboardGamemodes.Find(GorillaComputer.instance.currentGameMode.Value);
        if (mode != null && __instance.GetDesiredGameModeType() == mode.BaseMode)
            __result = mode.Id;
    }
}

[HarmonyPatch(typeof(GameMode), "FindGameModeInPropertyString")]
internal static class CardboardRoomModePatch
{
    private static void Postfix(string gmString, ref string __result)
    {
        // formatted as {MAP};{QUEUE};{GAMEMODE_STRING};;
        Debug.Log(gmString);
        if (CardboardGamemodes.Enabled && CardboardGamemodes.FromRoom(gmString) is { } mode)
            __result = mode.Id;
    }
}

[HarmonyPatch(typeof(GorillaComputer), "UpdateGameModeText")]
internal static class CardboardCurrentModeTextPatch
{
    private static void Postfix(GorillaComputer __instance)
    {
        if (!CardboardGamemodes.Enabled || NetworkSystem.Instance == null)
            return;

        if (NetworkSystem.Instance.InRoom && GorillaGameManager.instance != null)
            return;

        LocalisationManager.TryGetKeyForCurrentLocale("CURRENT_MODE", out string title, "CURRENT MODE");
        LocalisationManager.TryGetKeyForCurrentLocale("NOT_IN_ROOM", out string status, "-NOT IN ROOM-");
        __instance.currentGameModeText.Value = $"{title}\n{status}";
    }
}
