using BepInEx.Bootstrap;
using System.Linq;

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
    }
}
