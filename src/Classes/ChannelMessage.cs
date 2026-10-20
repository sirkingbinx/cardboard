using System;
using System.Linq;

namespace Cardboard.Classes;

/// <summary>
/// A message sent using a CardboardChannel.
/// </summary>
public class ChannelMessage
{
    /// <summary>
    /// The channel that the message was sent on.
    /// </summary>
    public string ChannelName;

    /// <summary>
    /// The type of message that was sent over the channel.
    /// </summary>
    public ChannelMessageType MessageType;

    /// <summary>
    /// An array of players who received the message.
    /// </summary>
    public NetPlayer[] Targets;

    /// <summary>
    /// The player who sent the message.
    /// </summary>
    public NetPlayer Sender;

    /// <summary>
    /// The data that was sent over the network.
    /// </summary>
    public object Data;
    
    /// <summary>
    /// Attempts to convert the message data into T, and returns if it was successful.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <param name="data">The output parameter to set. This will be the default value if conversion fails.</param>
    /// <returns>true if conversion is successful, otherwise false.</returns>
    public bool TryGetValue<T>(out T data)
    {
        if (Data is not T askingData)
        {
            data = default;
            return false;
        }

        data = askingData;
        return true;
    }

    internal static bool TryCreate(object[] data, int sendingActorNumber, out ChannelMessage message)
    {
        if ((data[0] is not string channelName) ||
            (data[1] is not int messageTypeValue) ||
            (data[2] is not int[] messageTargets))
        {
            message = null;
            return false;
        }

        ChannelMessageType messageType = (ChannelMessageType)messageTypeValue;

        if (!Enum.IsDefined(typeof(ChannelMessageType), messageType))
        {
            message = null;
            return false;
        }

        NetPlayer sender = NetworkSystem.Instance.AllNetPlayers
            .FirstOrDefault(n => sendingActorNumber == n.ActorNumber);

        if (sender is null)
        {
            message = null;
            return false;
        }

        message = new() {
            ChannelName = channelName,
            MessageType = messageType,
            Targets = [.. NetworkSystem.Instance.AllNetPlayers.Where(n => messageTargets.Contains(n.ActorNumber))],
            Sender = sender,
            Data = data[3],
        };

        return true;
    }

    private ChannelMessage() { }
}