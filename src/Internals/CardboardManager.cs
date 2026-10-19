using System;
using System.Runtime.InteropServices;
using Cardboard.Interfaces;
using GorillaNetworking;
using UnityEngine;

namespace Cardboard.Internals;

/// <summary>
/// This is an internal class. Don't use functions from here.
/// </summary>
internal class CardboardManager : MonoBehaviour
{
    private const string WelcomeMessage =
        $"Cardboard v{PluginInfo.Version}\n" +
        "(C) 2026 sirkingbinx" +
        "hello world";

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

        gameObject.AddComponent<CardboardPunCallbacks>();

        Logger = new CardboardLog("Cardboard");

        Logger.Log("Loading Cardboard");

        UserSystem system = new UserSystem();
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            system.SystemEnvironment = SystemEnvironment.Windows;
        else
            system.SystemEnvironment = SystemEnvironment.Unknown;

        if (system.SystemEnvironment == SystemEnvironment.Windows)
        {
            IntPtr ntdllHandle = GetModuleHandle("ntdll.dll");

            if (ntdllHandle != IntPtr.Zero)
            {
                IntPtr wineVersionProc = GetProcAddress(ntdllHandle, "wine_get_version");

                if (wineVersionProc != IntPtr.Zero)
                    system.SystemEnvironment = SystemEnvironment.WindowsOverWine;
            }
        }

        Logger.Log($"OS: {system.SystemEnvironment}");

        CardboardPlayer.System = system;

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

        Logger.Log($"Platform: {CardboardPlayer.Platform}");

        // Initialize event handlers

        foreach (var cHandler in CardboardReflection.GetInstancesOfInterface<ICardboardModdedHandler>())
        {
            CardboardModded.ModdedJoin += cHandler.OnModdedJoin;
            CardboardModded.ModdedLeave += cHandler.OnModdedLeave;
        }

        foreach (var pHandler in CardboardReflection.GetInstancesOfInterface<ICardboardPlayerHandler>())
        {
            CardboardEvents.OnPlayerJoinedRoom += pHandler.OnPlayerJoinedRoom;
            CardboardEvents.OnPlayerLeftRoom += pHandler.OnPlayerLeftRoom;
        }

        foreach (var lHandler in CardboardReflection.GetInstancesOfInterface<ICardboardLobbyHandler>())
        {
            CardboardEvents.OnJoinedRoom += lHandler.OnJoinedRoom;
            CardboardEvents.OnLeftRoom += lHandler.OnLeftRoom;
        }

        foreach (var gamemodeData in CardboardReflection.GetInstancesOfBepInterface<CardboardCustomGamemode>())
        {
            var gamemode = gamemodeData.Item2;

            CardboardLibIntegrator.Register(gamemodeData.Item1.Instance, // Plugin
                gamemode.Id, gamemode.Name, gamemode.BaseMode, // Metadata
                gamemode.OnJoined, gamemode.OnLeft // Actions
            );
        }

        Logger.Log(WelcomeMessage);
        
        CardboardEvents.FirePlayerSpawned();
        Logger.Log("Cardboard initialized successfully");
    }

    // wine detection
    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
}
