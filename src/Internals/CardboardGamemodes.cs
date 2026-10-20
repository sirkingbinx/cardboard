using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardboard.Modded;
using GorillaGameModes;
using GorillaNetworking;
using HarmonyLib;
using UnityEngine;

namespace Cardboard.Internals;

internal static class CardboardGamemodes
{
    internal sealed class Mode
    {
        internal readonly string Id;
        internal readonly string Name;
        internal readonly GameModeType BaseMode;
        internal readonly CardboardCustomGamemode Callbacks;

        internal Mode(string id, string name, GameModeType baseMode, CardboardCustomGamemode callbacks = null)
        {
            Id = id;
            Name = name;
            BaseMode = baseMode;
            Callbacks = callbacks;
        }
    }

    internal static readonly List<Mode> Modes = [];

    internal static bool Enabled { get; private set; }

    private static Mode joinedMode;
    private static string joinedRoomMode;

    internal static void Initialize(IEnumerable<CardboardCustomGamemode> customModes)
    {
        if (Enabled || CardboardLibIntegrator.HasProvider)
            return;

        foreach (GameModeType mode in Enum.GetValues(typeof(GameModeType)))
        {
            if (mode == GameModeType.None || mode >= GameModeType.Count ||
                mode == GameModeType.SuperCasual || mode == GameModeType.SuperInfect ||
                !GameMode.gameModeKeyByName.ContainsKey(mode.ToString()))
                continue;

            Add(CardboardModeProtocol.ModdedId(mode.ToString()), "MODDED " + DisplayName(mode), mode);
        }

        foreach (var mode in customModes)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mode.Name))
                    throw new ArgumentException("A mode name is required.");

                Add(CardboardModeProtocol.CustomId(mode.BaseMode.ToString(), mode.Id), mode.Name, mode.BaseMode, mode);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Could not register custom gamemode: {ex}");
            }
        }

        Enabled = true;
        new Harmony(PluginInfo.GUID + ".gamemodes").PatchAll(typeof(CardboardGamemodes).Assembly);
        NetworkSystem.Instance.OnJoinedRoomEvent += OnJoined;
        NetworkSystem.Instance.OnReturnedToSinglePlayer += OnLeft;

        CardboardManager.Instance.StartCoroutine(InitializeSelectors());
    }

    private static IEnumerator InitializeSelectors()
    {
        var computer = GorillaComputer.instance;

        while (computer == null || computer.currentGameMode == null || computer.currentGameModeText == null)
        {
            yield return null;
            computer = GorillaComputer.instance;
        }

        foreach (var layout in Resources.FindObjectsOfTypeAll<GameModeSelectorButtonLayout>())
        {
            if (layout.gameObject.scene.IsValid())
                Attach(layout);
        }

        AccessTools.Method(typeof(GorillaComputer), "UpdateGameModeText").Invoke(computer, null);
    }

    private static void Add(string id, string name, GameModeType baseMode, CardboardCustomGamemode callbacks = null)
    {
        if (!GameMode.gameModeKeyByName.TryGetValue(baseMode.ToString(), out int key))
            throw new ArgumentException($"Unregistered base mode: {baseMode}");

        if (GameMode.gameModeKeyByName.ContainsKey(id))
            throw new ArgumentException($"Duplicate mode ID: {id}");

        GameMode.gameModeKeyByName.Add(id, key);
        Modes.Add(new Mode(id, name, baseMode, callbacks));
    }

    internal static string DisplayName(GameModeType mode) =>
        (string)AccessTools.Method(typeof(GameModeZoneMapping), "GetModeName")
            .Invoke(GameMode.GameModeZoneMapping, new object[] { mode });

    internal static GameModeType Verify(GTZone zone, GameModeType mode)
    {
        if (GorillaComputer.instance.IsPlayerInVirtualStump())
            zone = GTZone.customMaps;

        // Super modes are selected directly instead of using the toggle.
        if ((mode == GameModeType.SuperCasual || mode == GameModeType.SuperInfect) &&
            GameMode.GameModeZoneMapping.GetModesForZone(zone, NetworkSystem.Instance.SessionIsPrivate).Contains(mode))
            return mode;

        return (GameModeType)AccessTools.Method(typeof(GameModeZoneMapping), "VerifyModeForZone").Invoke(
            GameMode.GameModeZoneMapping, new object[] { zone, mode, NetworkSystem.Instance.SessionIsPrivate });
    }

    internal static Mode Find(string id) => Modes.FirstOrDefault(mode => mode.Id == id);

    internal static Mode FromRoom(string room) => Find(CardboardModeProtocol.RoomMode(room));

    internal static void Attach(GameModeSelectorButtonLayout layout)
    {
        if (!layout.TryGetComponent<CardboardModeSelector>(out var selector))
            selector = layout.gameObject.AddComponent<CardboardModeSelector>();

        selector.Refresh();
    }

    private static void OnJoined()
    {
        OnLeft();

        joinedRoomMode = NetworkSystem.Instance.GameModeString;
        joinedMode = FromRoom(joinedRoomMode);

        try
        {
            joinedMode?.Callbacks?.OnJoined(joinedRoomMode);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static void OnLeft()
    {
        var mode = joinedMode;
        var room = joinedRoomMode;
        joinedMode = null;
        joinedRoomMode = null;

        try
        {
            mode?.Callbacks?.OnLeft(room);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
