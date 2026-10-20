using BepInEx.Bootstrap;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cardboard
{
    /// <summary>
    /// Holder for checking BepInEx plugins
    /// </summary>
    public static class CardboardBootstrap
    {
        /// <summary>
        /// Checks to see if mod by UUID is installed.
        /// </summary>
        /// <param name="uuid">The mod UUID to check.</param>
        /// <param name="version">Optionally, only return true if the version matches the argument provided.</param>
        /// <returns>Bool signifying if mod installed</returns>
        public static bool Installed(string uuid, string version = "*")
        {
            if (version == "*")
                return Chainloader.PluginInfos.ContainsKey(uuid);

            return Chainloader.PluginInfos.Where(k => k.Key == uuid && k.Value.Metadata.Version.ToString() == version).Any();
        }

        /// <summary>
        /// Attempts to get the BepInEx PluginInfo of an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search for.</param>
        /// <param name="pluginInfo">The output PluginInfo.</param>
        /// <returns>true if a PluginInfo was found, else false.</returns>
        public static bool TryGetPluginInfo(Assembly assembly, out BepInEx.PluginInfo pluginInfo)
        {
            KeyValuePair<BepInEx.PluginInfo, Assembly> found = BepInExAssemblies.FirstOrDefault(k => k.Value == assembly);
        
            if (found.Equals(default(KeyValuePair<BepInEx.PluginInfo, Assembly>)))
            {
                pluginInfo = null!;
                return false;
            }
            
            pluginInfo = found.Key;
            return true;
        }

        /// <summary>
        /// All assemblies that have been loaded by BepInEx accessible through their PluginInfo.
        /// </summary>
        public static Dictionary<BepInEx.PluginInfo, Assembly> BepInExAssemblies
        {
            get
            {
                if (field is null || field.Any())
                    field ??= Chainloader.PluginInfos.Values.Where(v => v.Instance != null).ToDictionary(v => v, v => v.Instance.GetType().Assembly);

                return field;
            }
        }
    }
}
