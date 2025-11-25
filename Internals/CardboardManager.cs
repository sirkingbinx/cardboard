using BepInEx.Bootstrap;
using Cardboard.Classes;
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
        internal static string welcomeMessage = @"

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
|| Cardboard v1.1.0                    ||
|| hello world!                        ||
=========================================

        ";

        internal static CardboardManager instance { get; private set; }

        void Start()
        {
            instance = this;
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

        void OnPlayerSpawned() {
            var platformTag = PlayFabAuthenticator.instance.platform.PlatformTag.ToLower();

            switch (platformTag) {
                default:
                case "steam":
                    CardboardPlayer.Platform = GamePlatform.Steam;
                    break;
                case "pc":
                    CardboardPlayer.Platform = GamePlatform.OculusRift;
                    break;
            }

            Debug.Log($"Cardboard platform: {platformTag} | {CardboardPlayer.Platform}");

            var modAssemblies = Chainloader.PluginInfos.Values
				.Select(pluginInfo => pluginInfo.Instance.GetType().Assembly).Distinct();
            var moddedHandlers = modAssemblies.SelectMany(assembly => assembly.GetTypes())
				.Where(type => typeof(ICardboardModdedHandler).IsAssignableFrom(type) && type.IsClass && !type.IsInterface);

            foreach (var moddedHandler in moddedHandlers) {
                var cHandler = Activator.CreateInstance(moddedHandler) as ICardboardModdedHandler;

                CardboardModded.ModdedJoin += cHandler.OnModdedJoin;
                CardboardModded.ModdedLeave += cHandler.OnModdedLeave;

                Debug.Log($"cb: Found instance of ICardboardModdedHandler at {moddedHandler.FullName}");
            }

            Debug.Log(welcomeMessage);
            CardboardEvents.FirePlayerSpawned();
        }
    }
}
