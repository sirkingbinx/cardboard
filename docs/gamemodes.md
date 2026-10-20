---
uid: gamemodes
title: Gamemodes
author: sirkingbinx
---

## Gamemodes
Cardboard has a simple custom gamemode system that makes it easy as ever to write your own gamemode logic. You can find an example of a gamemode below:

```cs
using Cardboard.Modded;
using GorillaGameModes;

namespace MyMod;

// Gamemodes implement CardboardCustomGamemode
public sealed class MyGamemode : CardboardCustomGamemode
{
    public string Id => "MY_GAMEMODE_ID"; // This ID needs to be unique 

    public string Name => "MY GAMEMODE"; // This appears on the gamemode selector

    // You can provide a custom gamemode to inherit here
    public GameModeType BaseMode => GameModeType.Casual;

    // Called when joining a room. Figure out if you are master client or just a player, and continue on.
    public virtual void OnJoined(string roomMode) { }

    // Do your cleanup here
    public virtual void OnLeft(string roomMode) { }
}
```

If you want more information about how CardboardCustomGamemode works, you can see the API documentation: @Cardboard.Modded.CardboardCustomGamemode