using System;
using System.Runtime.InteropServices;
using Cardboard.Interfaces;
using Cardboard.Utilities;
using GorillaNetworking;
using UnityEngine;

namespace Cardboard.Internals;

/// <summary>
/// This is an internal class. Don't use functions from here.
/// </summary>
internal class CardboardManager : MonoBehaviour
{
    internal static string WelcomeMessage = """
                                            =========================================
                                            ||                 _________________   ||
                                            ||   _____________/__              /   ||
                                            ||   \               \            /    ||
                                            ||    \               \__________/     ||
                                            ||     \               \    __-/ |     ||
                                            ||      -----------------__-     |     ||
                                            ||      |                |       |     ||
                                            ||      |                |       |     ||
                                            ||      |                |     __/     ||
                                            ||      |                |  __-        ||
                                            ||      |________________|_-           ||
                                            ||                                     ||
                                            =========================================
                                            || Cardboard v1.3.0                    ||
                                            || hello world!                        ||
                                            =========================================

                                            """;

    internal static CardboardManager Instance { get; private set; }

    internal CardboardLog Logger { get; private set; }

    private void Start()
    {
        Instance = this;

        GorillaTagger.OnPlayerSpawned(() =>
        {
            try
            {
                OnPlayerSpawned();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        });

        Logger = new CardboardLog("Cardboard");

        Logger.Log("Loading Cardboard");
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            CardboardPlayer.Environment = SystemEnvironment.Windows;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            CardboardPlayer.Environment = SystemEnvironment.Linux;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            CardboardPlayer.Environment = SystemEnvironment.Mac;
        else
            CardboardPlayer.Environment = SystemEnvironment.Unknown;

        Logger.Log($"os: {CardboardPlayer.Environment}");

        NetworkSystem.Instance.OnRaiseEvent += (eventCode, data, _) =>
        {
            if (eventCode != CardboardNetwork.CardboardEventCode)
                return;

            if (data is not object[] channelData || channelData.Length != 2)
                return;

            if (channelData[0] is not string channel)
                return;

            try
            {
                if (CardboardNetwork.eventHandlers.ContainsKey(channel))
                    CardboardNetwork.eventHandlers[channel].Invoke(channelData[1]);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        };
    }

    private void OnPlayerSpawned()
    {
        var platformTag = PlayFabAuthenticator.instance.platform.ToString().ToLower();

        CardboardPlayer.Platform = platformTag switch
        {
            "steam" => GamePlatform.Steam,
            "pc" => GamePlatform.OculusRift,
            _ => GamePlatform.None
        };

        Logger.Log($"platform: {platformTag} | {CardboardPlayer.Platform}");

        // Initialize event handlers

        foreach (var cHandler in CardboardReflection.GetInstancesOfInterface<ICardboardModdedHandler>()) {
            CardboardModded.ModdedJoin += cHandler.OnModdedJoin;
            CardboardModded.ModdedLeave += cHandler.OnModdedLeave;
        }

        foreach (var pHandler in CardboardReflection.GetInstancesOfInterface<ICardboardPlayerHandler>()) {
            CardboardEvents.OnPlayerJoinedRoom += pHandler.OnPlayerJoinedRoom;
            CardboardEvents.OnPlayerLeftRoom += pHandler.OnPlayerLeftRoom;
        }

        foreach (var lHandler in CardboardReflection.GetInstancesOfInterface<ICardboardLobbyHandler>()) {
            CardboardEvents.OnJoinedRoom += lHandler.OnJoinedRoom;
            CardboardEvents.OnLeftRoom += lHandler.OnLeftRoom;
        }

        Logger.Log(WelcomeMessage);
        
        CardboardEvents.FirePlayerSpawned();
        Logger.Log("Cardboard initialized successfully");
    }
}
