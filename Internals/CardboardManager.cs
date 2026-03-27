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
                                                || Cardboard v1.3.0                    ||
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

            foreach (var cHandler in GetInstancesOf<ICardboardModdedHandler>()) {
                CardboardModded.ModdedJoin += cHandler.OnModdedJoin;
                CardboardModded.ModdedLeave += cHandler.OnModdedLeave;
            }

            foreach (var pHandler in GetInstancesOf<ICardboardPlayerHandler>()) {
                CardboardEvents.OnPlayerJoinedRoom += pHandler.OnPlayerJoinedRoom;
                CardboardEvents.OnPlayerLeftRoom += pHandler.OnPlayerLeftRoom;
            }

            foreach (var lHandler in GetInstancesOf<ICardboardLobbyHandler>()) {
                CardboardEvents.OnJoinedRoom += pHandler.OnJoinedRoom;
                CardboardEvents.OnLeftRoom += pHandler.OnLeftRoom;
            }

            Debug.Log(WelcomeMessage);
            CardboardEvents.FirePlayerSpawned();
        }

        private static List<T> GetInstancesOf<T>() {
            var modAssemblies = Chainloader.PluginInfos.Values
				.Select(pluginInfo => pluginInfo.Instance.GetType().Assembly).Distinct();
            var types = modAssemblies.SelectMany(assembly => assembly.GetTypes())
				.Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsInterface);

            var typeInstances = new List<T>();

            foreach (var type in types) {
                if (Activator.CreateInstance(type) is not T typeInstance)
                    continue;

                typeInstances.Add(type);
            }

            return type;
        }
    }
}
