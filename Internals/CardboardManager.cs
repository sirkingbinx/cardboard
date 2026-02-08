using BepInEx.Bootstrap;
using Cardboard.Interfaces;
using Cardboard.Utils;
using GorillaNetworking;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Cardboard.Internals
{
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
                                                || Cardboard v1.2.0                    ||
                                                || hello world!                        ||
                                                =========================================

                                                """;

        internal static CardboardManager Instance { get; private set; }

        private void Start()
        {
            Instance = this;
            GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
            CardboardConfig.UpdateConfig();
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                CardboardPlayer.Environment = SystemEnvironment.Windows;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                CardboardPlayer.Environment = SystemEnvironment.Linux;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                CardboardPlayer.Environment = SystemEnvironment.Mac;
            else
                CardboardPlayer.Environment = SystemEnvironment.Unknown;
        }

        private static void OnPlayerSpawned() {
            var platformTag = PlayFabAuthenticator.instance.platform.PlatformTag.ToLower();

            CardboardPlayer.Platform = platformTag switch
            {
                "steam" => GamePlatform.Steam,
                "pc" => GamePlatform.OculusRift,
                _ => GamePlatform.None
            };

            Debug.Log($"Cardboard platform: {platformTag} | {CardboardPlayer.Platform}");

            var modAssemblies = Chainloader.PluginInfos.Values
				.Select(pluginInfo => pluginInfo.Instance.GetType().Assembly).Distinct();
            var moddedHandlers = modAssemblies.SelectMany(assembly => assembly.GetTypes())
				.Where(type => typeof(ICardboardModdedHandler).IsAssignableFrom(type) && type.IsClass && !type.IsInterface);

            foreach (var moddedHandler in moddedHandlers) {
                if (Activator.CreateInstance(moddedHandler) is not ICardboardModdedHandler cHandler)
                    continue;

                CardboardModded.ModdedJoin += cHandler.OnModdedJoin;
                CardboardModded.ModdedLeave += cHandler.OnModdedLeave;

                Debug.Log($"cb: Found instance of ICardboardModdedHandler at {moddedHandler.FullName}");
            }

            Debug.Log(WelcomeMessage);
            CardboardEvents.FirePlayerSpawned();
        }
    }
}
