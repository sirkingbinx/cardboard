using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using Cardboard.Interfaces;
using Cardboard.Modded;
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

    private bool initialized;

    private void Start()
    {
        Instance = this;
        Logger = new CardboardLog("Cardboard");

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

        Logger.Log("Loading Cardboard");

        UserSystem system = new UserSystem();
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            system.SystemEnvironment = SystemEnvironment.Windows;
        else
            system.SystemEnvironment = SystemEnvironment.Unknown;

        if (system.SystemEnvironment == SystemEnvironment.Windows)
        {
            try
            {
                IntPtr ntdllHandle = GetModuleHandle("ntdll.dll");

                if (ntdllHandle != IntPtr.Zero)
                {
                    IntPtr wineVersionProc = GetProcAddress(ntdllHandle, "wine_get_version");

                    if (wineVersionProc != IntPtr.Zero)
                        system.SystemEnvironment = SystemEnvironment.Linux;
                }
            } catch { } // not on wine, ignore
        }

        Logger.Log($"OS: {system.SystemEnvironment}");

        CardboardPlayer.System = system;
    }

    private void OnPlayerSpawned()
    {
        if (initialized)
            return;

        initialized = true;
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

        var customModes = CardboardReflection.GetInstancesOfInterface<CardboardCustomGamemode>();
        if (CardboardLibIntegrator.HasProvider)
        {
            foreach (CardboardCustomGamemode mode in customModes)
            {
                if (!CardboardBootstrap.TryGetPluginInfo(mode.GetType().Assembly, out BepInEx.PluginInfo info))
                    continue;

                StartCoroutine(RegisterWithProvider(info.Instance, mode));
            }
        }
        else
        {
            CardboardGamemodes.Initialize(customModes);
        }

        Logger.Log(WelcomeMessage);
        
        CardboardEvents.FirePlayerSpawned();
        Logger.Log("Cardboard initialized successfully");
    }

    private IEnumerator RegisterWithProvider(BepInEx.BaseUnityPlugin owner, CardboardCustomGamemode mode)
    {
        // Wait for Utilla/GL to finish loading.
        while (true)
        {
            string id;

            try
            {
                id = CardboardLibIntegrator.Register(owner, CardboardModeProtocol.CustomId(string.Empty, mode.Id), mode.Name,
                    mode.BaseMode, mode.OnJoined, mode.OnLeft);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                yield break;
            }

            if (id != null)
                yield break;

            yield return null;
        }
    }

    // wine detection
    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
}
