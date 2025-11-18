using BepInEx.Bootstrap;

namespace Cardboard.Utils
{
    /// <summary>
    /// Holder for checking BepInEx plugins
    /// </summary>
    public static class CardboardBootstrap
    {
        /// <summary>
        /// Checks to see if mod by UUID is installed.
        /// </summary>
        /// <param name="UUID">The mod UUID to check.</param>
        /// <returns>Bool signifying if mod installed</returns>
        public static bool Installed(string _UUID) =>
            Chainloader.PluginInfos.Keys.ContainsKey(_UUID);
    }
}
