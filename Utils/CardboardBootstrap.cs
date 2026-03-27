using System;

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
        /// <param name="_UUID">The mod UUID to check.</param>
        /// <returns>Bool signifying if mod installed</returns>
        [Obsolete("No longer maintained, please manually check for other mods being injected", true)]
        public static bool Installed(string _UUID) => false;
    }
}
