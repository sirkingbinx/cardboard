using System;
using System.Collections.Generic;
using System.Linq;
using Cardboard.Internals;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Cardboard;

/// <summary>
/// Channels can send and receive data across the network.
/// </summary>
public sealed class CardboardChannel : IDisposable
{
    /// <summary>
    /// The name of the channel.
    /// </summary>
    public string Name;

    /// <summary>
    /// A channel message delegate type.
    /// </summary>
    /// <param name="data">The data sent across the channel.</param>
    /// <param name="sender">The sender of the message.</param>
    public delegate void ChannelMessage(object data, NetPlayer sender);

    /// <summary>
    /// Action called when the channel receives a message.
    /// </summary>
    public ChannelMessage OnReceivedMessage = (_, _) => {};

    /// <summary>
    /// Send an object on the channel to a player.
    /// </summary>
    /// <param name="data">The object to send.</param>
    /// <param name="targetPlayer">The player that should receive the object.</param>
    /// <exception cref="ArgumentNullException">channel or targetPlayer is null.</exception>
    /// <exception cref="Exception">Most likely thrown if not connected to a room.</exception>
    public void SendMessage(object data, NetPlayer targetPlayer)
    {
        if (targetPlayer == null)
            throw new ArgumentNullException(nameof(targetPlayer));
        
        if (!NetworkSystem.Instance.InRoom)
            throw new Exception("You are not connected to a room.");
            
        object[] sendData = [Name, data];

        RaiseEventOptions raiseEventOptions = new()
        {
            TargetActors = [ targetPlayer.ActorNumber ]
        };

        PhotonNetwork.RaiseEvent(CardboardNetwork.CardboardEventCode, sendData, raiseEventOptions, SendOptions.SendReliable);
    }

    /// <summary>
    /// Send an object on the channel to players.
    /// </summary>
    /// <param name="data">The object to send.</param>
    /// <param name="targetPlayers">The players that should receive the object.</param>
    /// <exception cref="ArgumentNullException">channel or targetPlayer is null.</exception>
    /// <exception cref="Exception">Most likely thrown if not connected to a room.</exception>
    public void SendMessage(object data, IEnumerable<NetPlayer> targetPlayers)
    {
        if (targetPlayers == null)
            throw new ArgumentNullException(nameof(targetPlayers));
        
        if (!NetworkSystem.Instance.InRoom)
            throw new Exception("You are not connected to a room.");
            
        object[] sendData = [Name, data];

        RaiseEventOptions raiseEventOptions = new()
        {
            TargetActors = [ .. targetPlayers.Select(player => player.ActorNumber) ]
        };

        PhotonNetwork.RaiseEvent(CardboardNetwork.CardboardEventCode, sendData, raiseEventOptions, SendOptions.SendReliable);
    }

    /// <summary>
    /// Send an object on the channel to all players in the room.
    /// </summary>
    /// <param name="data">The object to send.</param>
    /// <exception cref="ArgumentNullException">channel or targetPlayer is null.</exception>
    /// <exception cref="Exception">Most likely thrown if not connected to a room.</exception>
    public void SendMessage(object data)
    {
        if (!NetworkSystem.Instance.InRoom)
            throw new Exception("You are not connected to a room.");
            
        object[] sendData = [Name, data];

        RaiseEventOptions raiseEventOptions = new()
        {
            TargetActors = [ .. NetworkSystem.Instance.PlayerListOthers.Select(player => player.ActorNumber) ]
        };

        PhotonNetwork.RaiseEvent(CardboardNetwork.CardboardEventCode, sendData, raiseEventOptions, SendOptions.SendReliable);
    }

    /// <inheritdoc cref="CardboardChannel"/>
    /// <param name="channelName">The name of the channel.</param>
    public CardboardChannel(string channelName)
    {
        Name = channelName;

        NetworkSystem.Instance.OnRaiseEvent += OnRaiseEvent;
    }

    private void OnRaiseEvent(byte eventCode, object data, int callingActor)
    {
        if (eventCode != CardboardNetwork.CardboardEventCode)
            return;

        if (data is not object[] channelData || channelData.Length != 2)
            return;

        if (channelData[0] is not string channel)
            return;

        var players = NetworkSystem.Instance.AllNetPlayers;
        if (players.FirstOrDefault(n => n.ActorNumber == callingActor) is not NetPlayer invoker)
            return;

        try
        {
            if (Name == channel)
                OnReceivedMessage?.Invoke(channelData[1], invoker);
        }
        catch (Exception ex)
        {
            CardboardManager.Instance.Logger.Log(ex);
        }
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        NetworkSystem.Instance.OnRaiseEvent -= OnRaiseEvent;
    }
}