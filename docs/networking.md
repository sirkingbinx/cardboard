---
uid: networking
title: Networking
author: sirkingbinx
---

## Networking
Cardboard has its own simple networking system called the "Channel system". It is built on top of Photon events and makes networking simplified for new modders.

## Setting up communication
```cs
using Cardboard;
using Cardboard.Interfaces;

public class LobbyNetworkingLogic : ICardboardModdedHandler
{
    public CardboardChannel commChannel;

    public void OnModdedJoin() 
    {
        // Sets up channels when you join a modded lobby
        commChannel = new CardboardChannel("mod-comm-channel");
        commChannel.OnReceivedMessage += OnCommChannelMessage;

        // Extra note: channels implement IDisposable, so you can also use them inside of `using` statements; see below
        using CardboardChannel temporaryChannel = new CardboardChannel("i-am-sending-something");

        // Here's how to send data. It does not have to be a string, it can be anything
        temporaryChannel.SendMessage("Hello world"); // Send a message to everyone
        temporaryChannel.SendMessage("Hello world", [ player1, player2, player3 ]); // Send a message to a list of players
        temporaryChannel.SendMessage("Hello world", specificNetPlayer); // Send a message to a specific person
    }

    public void OnModdedLeave()
    {
        // Unhooks your channel when a modded server is left
        commChannel.Dispose();
    }

    private void OnCommChannelMessage(ChannelMessage message)
    {
        // You can use this helper method
        if (message.TryGetValue(out string myMessage))
            Debug.Log(myMessage);
        
        // Or manually cast your message data
        if (message.Data is string myManualMessage)
            Debug.Log(myManualMessage);

        var receivers = message.Targets; // everyone who received this message
        var sender = message.Sender; // who sent the message

        // either Broadcast, Multicast, or Unicast
        // (everyone, multiple people, one person)
        var messageType = message.MessageType;
    }
}
```

For more info on channels, see @Cardboard.CardboardChannel.