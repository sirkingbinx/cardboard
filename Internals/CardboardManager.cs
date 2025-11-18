using UnityEngine;
using Cardboard.Attributes;
using Cardboard.Interfaces;
using Cardboard.Utils;
using GorillaNetworking;

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
            GorillaTagger.OnPlayerInitialized(OnPlayerSpawned);
            CardboardConfig.UpdateConfig();
        }

        void OnPlayerSpawned() {
            switch (PlayFabAuthenticator.instance.platform.PlatformTag.ToLower()) {
                default:
                case "steam":
                    CardboardPlayer.Platform = GamePlatform.Steam;
                    break;
                case "pc":
                    CardboardPlayer.Platform = GamePlatform.OculusRift;
                    break;
            }

            var modAssemblies = Chainloader.PluginInfos.Values
				.Select(pluginInfo => pluginInfo.Instance.GetType().Assembly).Distinct();
            var moddedHandlers = modAssemblies.SelectMany(assembly => assembly.GetTypes())
				.Where(type => typeof(ICardboardModdedHandler).IsAssignableFrom(type) && type.IsClass && !type.IsInterface);
            var patchAutomatically = modAssemblies.SelectMany(assembly => assembly.GetTypes())
				.Where(type => type.IsDefined(typeof(CardboardAutoPatch), false) && type.IsClass && type.IsAssignableFrom(typeof(BaseUnityPlugin)) && !type.IsInterface);

            foreach (var moddedHandler in moddedHandlers) {
                var cHandler = Activator.CreateInstance(moddedHandler) as ICardboardModdedHandler;

                CardboardModded.OnModdedJoin += cHandler.OnModdedJoin;
                CardboardModded.OnModdedLeave += cHandler.OnModdedLeave;

                cHandler.OnPlayerInitialized();
            }

            foreach (var autoPatch in patchAutomatically) {
                if (autoPatch != null)
                    CardboardHarmony.PatchInstance(autoPatch);
                else
                    // I'm sorry for this error message.
                    Debug.LogWarning($"warning - {autoPatch.FullName}: Plugin uses [CardboardAutoPatch] but does not have Cardboard as a hard dependency. Fix this by adding the following attribute to your BaseUnityPlugin:\n\n\t[BepInDependency(\"bingus.cardboard\", DependencyFlags.HardDependency)]\n\n");
            }

            Debug.Log(welcomeMessage);
        }
    }
}
