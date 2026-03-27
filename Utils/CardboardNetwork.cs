using System;
using ExitGames.Client.Photon;
using Photon.Pun;

namespace Cardboard.Utils
{
    /// <summary>
    /// Util class for networking properties.
    /// </summary>
    public static class CardboardNetwork
    {
        /// <summary>
        /// Adds a property to the PhotonNetwork LocalPlayer.
        /// </summary>
        /// <param name="key">Key of the property to add.</param>
        /// <param name="value">Value of the property to add.</param>
        [Obsolete("Networking properties has become unreliable in recent versions of Gorilla Tag; consider migrating to event-based networking.")]
        public static void CreateProperty(string key, string value) =>
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
            { {key, value} });

        /// <summary>
        /// Returns the properties of the local player.
        /// </summary>
        /// <returns>The custom properties list of the LocalPlayer.</returns>
        public static Hashtable GetProperties() =>
            PhotonNetwork.LocalPlayer.CustomProperties;

        /// <summary>
        /// Returns the properties of the provided NetPlayer.
        /// </summary>
        /// <param name="player">The player to get the properties for.</param>
        /// <returns>The custom properties list of the LocalPlayer.</returns>
        public static Hashtable GetProperties(NetPlayer player) =>
            player.GetPlayerRef().CustomProperties;
        
        /// <summary>
        /// Returns the value of the provided key in the localplayer's properties.
        /// </summary>
        /// <param name="key">The value of key inside of the player properties.</param>
        /// <returns>Value of the key in properties</returns>
        public static object GetPlayerProperty(string key) =>
            PhotonNetwork.LocalPlayer.CustomProperties[key];

        /// <summary>
        /// Returns the value of the provided key in the player's properties.
        /// </summary>
        /// <param name="player">The player to get the properties for.</param>
        /// <param name="key">The value of key inside of the player properties.</param>
        /// <returns>Value of the key in properties</returns>
        public static object GetPlayerProperty(NetPlayer player, string key) =>
            player.GetPlayerRef().CustomProperties[key];
    }
}
