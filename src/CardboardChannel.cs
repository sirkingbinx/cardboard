using System;
using System.Collections.Generic;
using System.Linq;
using Cardboard.Classes;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

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
    /// <param name="message">The message that was received.</param>
    public delegate void ChannelMessageHandler(ChannelMessage message);

    /// <summary>
    /// Action called when the channel receives a message.
    /// </summary>
    public ChannelMessageHandler OnReceivedMessage = (_) => {};

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
            
        int[] targetActorNumbers = [ targetPlayer.ActorNumber ];
        object[] sendData = [Name, (int)ChannelMessageType.Unicast, targetActorNumbers, data];

        RaiseEventOptions raiseEventOptions = new()
        {
            TargetActors = targetActorNumbers
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

        var players = targetPlayers.ToArray();

        if (players.Any(n => n is null))
            throw new ArgumentNullException(nameof(targetPlayers));
        
        if (!NetworkSystem.Instance.InRoom)
            throw new Exception("You are not connected to a room.");
            
        int[] targetActorNumbers = [ .. players.Select(player => player.ActorNumber) ];
        object[] sendData = [Name, (int)ChannelMessageType.Multicast, targetActorNumbers, data];

        RaiseEventOptions raiseEventOptions = new()
        {
            TargetActors = targetActorNumbers
        };

        PhotonNetwork.RaiseEvent(CardboardNetwork.CardboardEventCode, sendData, raiseEventOptions, SendOptions.SendReliable);
    }

    /// <summary>
    /// Send an object on the channel to all players in the room.
    /// </summary>
    /// <param name="data">The object to send.</param>
    /// <param name="sendSelf">Send the message to yourself</param>
    /// <exception cref="ArgumentNullException">channel or targetPlayer is null.</exception>
    /// <exception cref="Exception">Most likely thrown if not connected to a room.</exception>
    public void SendMessage(object data, bool sendSelf = false)
    {
        if (!NetworkSystem.Instance.InRoom)
            throw new Exception("You are not connected to a room.");
        
        int[] targetActorNumbers = [ .. NetworkSystem.Instance.PlayerListOthers.Select(player => player.ActorNumber) ];
        object[] sendData = [Name, (int)ChannelMessageType.Broadcast, targetActorNumbers, data];

        RaiseEventOptions raiseEventOptions = new()
        {
            TargetActors = targetActorNumbers
        };

        if (sendSelf)
            sendData[2] = NetworkSystem.Instance.AllNetPlayers.Select(player => player.ActorNumber).ToArray();

        PhotonNetwork.RaiseEvent(CardboardNetwork.CardboardEventCode, sendData, raiseEventOptions, SendOptions.SendReliable);

        if (!sendSelf)
            return;
        
        OnRaiseEvent(CardboardNetwork.CardboardEventCode, sendData, NetworkSystem.Instance.LocalPlayer.ActorNumber);
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

        if (data is not object[] channelData || channelData.Length != 4)
            return;

        if (channelData[0] is not string channel || channel != Name)
            return;

        ChannelMessage message;

        if (!ChannelMessage.TryCreate(channelData, callingActor, out message))
            return;

        try { OnReceivedMessage?.Invoke(message); }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        NetworkSystem.Instance.OnRaiseEvent -= OnRaiseEvent;
    }
}