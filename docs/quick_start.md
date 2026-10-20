---
uid: quick_start
title: Quick Start
author: sirkingbinx
---

## Quick Start
Cardboard contains many different classes, methods, events and more you can use to simplify your mod's development and update process. For example, a couple of the things Cardboard can do are:

- **Custom gamemodes**: If your userbase is mixed between Utilla and GorillaLibrary, Cardboard can work with both to add your gamemode, or manage its own gamemode systems if neither are installed.
- **Reflection**: If your mod does a lot of .NET reflection, Cardboard has helper classes for handling reflection efficently.
- **Networking**: Cardboard makes networking simple using the Channel system.
- **Linux/Windows support**: Cardboard natively handles paths for Linux and Windows, so you can have consistent behaviour across both platforms.

## A basic plugin
```cs
using BepInEx;
using Cardboard;

[BepInPlugin("com.myname.mymod", "MyMod", "1.0.0")]
public class MyPlugin : BaseUnityPlugin
{
    private static CardboardLog Log;

    private void Awake()
    {
        Log = new CardboardLog(this);
        CardboardEvents.OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnEnable()
    {
        // Apply Harmony patches automatically
        CardboardHarmony.PatchInstance(this);
    }

    private void OnDisable()
    {
        // Unapply Harmony patches. No arguments required!
        CardboardHarmony.UnpatchInstance();
    }

    private void OnPlayerSpawned()
    {
        Log.Write("Hello world!");
    }
}
```

The only references in your code were to Cardboard and BepInEx, and you have already done the following:
- Set up applying/unapplying your Harmony patches
- Set up your own custom logs
- Hooked a method onto Gorilla Tag's OnPlayerSpawned event

Most of Gorilla Tag modding's repetitive tasks have now been simplified to 30 lines of code, *including* whitespace and corrected formatting.