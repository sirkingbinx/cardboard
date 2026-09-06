using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Cardboard.Utilities
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
                { { key, value } });

        /// <summary>
        /// Returns the properties of the local player.
        /// </summary>
        /// <returns>The custom properties list of the LocalPlayer.</returns>
        [Obsolete("Networking properties has become unreliable in recent versions of Gorilla Tag; consider migrating to event-based networking.")]
        public static Hashtable GetProperties() =>
            PhotonNetwork.LocalPlayer.CustomProperties;

        /// <summary>
        /// Returns the properties of the provided NetPlayer.
        /// </summary>
        /// <param name="player">The player to get the properties for.</param>
        /// <returns>The custom properties list of the LocalPlayer.</returns>
        [Obsolete("Networking properties has become unreliable in recent versions of Gorilla Tag; consider migrating to event-based networking.")]
        public static Hashtable GetProperties(NetPlayer player) =>
            player.GetPlayerRef().CustomProperties;

        /// <summary>
        /// Returns the value of the provided key in the localplayer's properties.
        /// </summary>
        /// <param name="key">The value of key inside of the player properties.</param>
        /// <returns>Value of the key in properties</returns>
        [Obsolete("Networking properties has become unreliable in recent versions of Gorilla Tag; consider migrating to event-based networking.")]
        public static object GetPlayerProperty(string key) =>
            PhotonNetwork.LocalPlayer.CustomProperties[key];

        /// <summary>
        /// Returns the value of the provided key in the player's properties.
        /// </summary>
        /// <param name="player">The player to get the properties for.</param>
        /// <param name="key">The value of key inside of the player properties.</param>
        /// <returns>Value of the key in properties</returns>
        [Obsolete("Networking properties has become unreliable in recent versions of Gorilla Tag; consider migrating to event-based networking.")]
        public static object GetPlayerProperty(NetPlayer player, string key) =>
            player.GetPlayerRef().CustomProperties[key];

        internal const byte CardboardEventCode = 26;
        internal static Dictionary<string, Action<object>> eventHandlers = new();
        
        /// <summary>
        /// Register a callback for when an object is sent on the specified channel.
        /// </summary>
        /// <param name="channel">The channel to listen to events on.</param>
        /// <param name="callback">The action to fire when an object is sent.</param>
        /// <exception cref="ArgumentNullException">An argument is null.</exception>
        public static void AddObjectHandler(string channel, Action<object> callback)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));
            
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            
            eventHandlers.TryAdd(channel, callback);
        }
        
        /// <summary>
        /// Send an object on the specified channel to a player.
        /// </summary>
        /// <param name="channel">The channel to send the object on.</param>
        /// <param name="data">The object to send.</param>
        /// <param name="targetPlayer">The player that should receive the object.</param>
        /// <exception cref="ArgumentNullException">channel or targetPlayer is null.</exception>
        /// <exception cref="Exception">Most likely thrown if not connected to a room.</exception>
        public static void SendObject(string channel, object data, NetPlayer targetPlayer)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));
            
            if (targetPlayer == null)
                throw new ArgumentNullException(nameof(targetPlayer));
            
            if (!NetworkSystem.Instance.InRoom)
                throw new Exception("You are not connected to a room.");
                
            object[] sendData = [channel, data];

            RaiseEventOptions raiseEventOptions = new()
            {
                TargetActors = [ targetPlayer.ActorNumber ]
            };

            PhotonNetwork.RaiseEvent(CardboardEventCode, sendData, raiseEventOptions, SendOptions.SendReliable);
        }
        
        /// <summary>
        /// Send an object on the specified channel to multiple players.
        /// </summary>
        /// <param name="channel">The channel to send the object on.</param>
        /// <param name="data">The object to send.</param>
        /// <param name="targetPlayers">The players that should receive the object.</param>
        /// <exception cref="ArgumentNullException">channel or targetPlayers is null.</exception>
        /// <exception cref="Exception">Most likely thrown if not connected to a room.</exception>
        public static void SendObject(string channel, object data, NetPlayer[] targetPlayers)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));
            
            if (targetPlayers == null)
                throw new ArgumentNullException(nameof(targetPlayers));
            
            if (!NetworkSystem.Instance.InRoom)
                throw new Exception("You are not connected to a room.");
            
            object[] sendData = [channel, data];

            RaiseEventOptions raiseEventOptions = new()
            {
                TargetActors = [ .. targetPlayers.Select(player => player.ActorNumber) ]
            };

            PhotonNetwork.RaiseEvent(CardboardEventCode, sendData, raiseEventOptions, SendOptions.SendReliable);
        }
        
        /// <summary>
        /// Send an object on the specified channel to every player in the room.
        /// </summary>
        /// <param name="channel">The channel to send the object on.</param>
        /// <param name="data">The object to send.</param>
        /// <exception cref="ArgumentNullException">channel is null.</exception>
        /// <exception cref="Exception">Most likely thrown if not connected to a room.</exception>
        public static void SendObject(string channel, object data)
        {
            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException(nameof(channel));
            
            if (!NetworkSystem.Instance.InRoom)
                throw new Exception("You are not connected to a room.");
            
            object[] sendData = [channel, data];

            RaiseEventOptions raiseEventOptions = new()
            {
                TargetActors = [ .. NetworkSystem.Instance.PlayerListOthers.Select(player => player.ActorNumber) ]
            };

            PhotonNetwork.RaiseEvent(CardboardEventCode, sendData, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
