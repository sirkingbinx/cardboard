using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using BepInEx.Bootstrap;
using Cardboard.Bootstrap;
using Cardboard.Interfaces;
using Cardboard.Utilities;
using GorillaNetworking;
using UnityEngine;

namespace Cardboard.Internals;

/// <summary>
/// This is an internal class. Don't use functions from here.
/// </summary>
internal class CardboardManager : MonoBehaviour
{
    internal static string WelcomeMessage = """
                                            =========================================
                                            ||                 _________________   ||
                                            ||   _____________/__              /   ||
                                            ||   \               \            /    ||
                                            ||    \               \__________/     ||
                                            ||     \               \    __-/ |     ||
                                            ||      -----------------__-     |     ||
                                            ||      |                |       |     ||
                                            ||      |                |       |     ||
                                            ||      |                |     __/     ||
                                            ||      |                |  __-        ||
                                            ||      |________________|_-           ||
                                            ||                                     ||
                                            =========================================
                                            || Cardboard v1.3.0                    ||
                                            || hello world!                        ||
                                            =========================================

                                            """;

    internal static CardboardManager Instance { get; private set; }

    internal CardboardLog Logger { get; private set; }

    private void Start()
    {
        Instance = this;

        GorillaTagger.OnPlayerSpawned(() =>
        {
            try
            {
                OnPlayerSpawned();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        });

        Logger = new CardboardLog("Cardboard");

        Logger.Log("Loading Cardboard");
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            CardboardPlayer.Environment = SystemEnvironment.Windows;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            CardboardPlayer.Environment = SystemEnvironment.Linux;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            CardboardPlayer.Environment = SystemEnvironment.Mac;
        else
            CardboardPlayer.Environment = SystemEnvironment.Unknown;

        Logger.Log($"os: {CardboardPlayer.Environment}");
    }

    private void OnPlayerSpawned()
    {
        var platformTag = PlayFabAuthenticator.instance.platform.ToString().ToLower();

        CardboardPlayer.Platform = platformTag switch
        {
            "steam" => GamePlatform.Steam,
            "pc" => GamePlatform.OculusRift,
            _ => GamePlatform.None
        };

        Logger.Log($"platform: {platformTag} | {CardboardPlayer.Platform}");

        // Initialize CardboardMods

        LoadAvaliableAssemblies();

        foreach (var mod in GetInstancesOfTypeWithAttribute<ModInfo, CardboardMod>())
        {
            ModInfo modInfo = mod.Item1;
            CardboardMod cardboardMod = mod.Item2;

            cardboardMod.Info = modInfo;
            cardboardMod.ModLoader = Constants.Loader;

            CardboardRequirement[] requirements = [.. cardboardMod.GetType().GetCustomAttributes<CardboardRequirement>(false)];

        }

        // Initialize event handlers

        foreach (var cHandler in GetInstancesOfInterface<ICardboardModdedHandler>()) {
            CardboardModded.ModdedJoin += cHandler.OnModdedJoin;
            CardboardModded.ModdedLeave += cHandler.OnModdedLeave;
        }

        foreach (var pHandler in GetInstancesOfInterface<ICardboardPlayerHandler>()) {
            CardboardEvents.OnPlayerJoinedRoom += pHandler.OnPlayerJoinedRoom;
            CardboardEvents.OnPlayerLeftRoom += pHandler.OnPlayerLeftRoom;
        }

        foreach (var lHandler in GetInstancesOfInterface<ICardboardLobbyHandler>()) {
            CardboardEvents.OnJoinedRoom += lHandler.OnJoinedRoom;
            CardboardEvents.OnLeftRoom += lHandler.OnLeftRoom;
        }

        Logger.Log(WelcomeMessage);
        
        CardboardEvents.FirePlayerSpawned();
        Logger.Log("Cardboard initialized successfully");
    }

    private static List<T> GetInstancesOfInterface<T>()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes())
		    .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface);

        var typeInstances = new List<T>();

        foreach (var type in types) {
            if (Activator.CreateInstance(type) is not T typeInstance)
                continue;

            typeInstances.Add(typeInstance);
        }

        return typeInstances;
    }

    private static List<(T1, T2)> GetInstancesOfTypeWithAttribute<T1, T2>() where T1 : Attribute
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T2).IsAssignableFrom(type) && type.GetCustomAttributes<T1>(true).Any() && !type.IsInterface);

        var typeInstances = new List<(T1, T2)>();

        foreach (var type in types)
        {
            if (Activator.CreateInstance(type) is not T2 typeInstance)
                continue;

            T1 attribute = typeInstance.GetType().GetCustomAttribute<T1>();

            typeInstances.Add((attribute, typeInstance));
        }

        return typeInstances;
    }

    private static void LoadAvaliableAssemblies()
    {
        string CustomPluginsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cardboard");

        if (!Directory.Exists(CustomPluginsDirectory))
        {
            Directory.CreateDirectory(CustomPluginsDirectory);
            return;
        }
        
        string BepInExPluginsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepInEx", "plugins");
        string MelonLoaderPluginsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods");

        if (Directory.Exists(BepInExPluginsDirectory))
        {
            string[] assemblies = Directory.GetFiles(BepInExPluginsDirectory, "*.dll", SearchOption.AllDirectories);
            assemblies.ForEach(AddAssemblyToDomain);
        }

        if (Directory.Exists(MelonLoaderPluginsDirectory))
        {
            string[] assemblies = Directory.GetFiles(MelonLoaderPluginsDirectory, "*.dll", SearchOption.AllDirectories);
            assemblies.ForEach(AddAssemblyToDomain);
        }

        string[] cardboardAssemblies = Directory.GetFiles(CustomPluginsDirectory, "*.dll", SearchOption.AllDirectories);
        cardboardAssemblies.ForEach(AddAssemblyToDomain);
    }

    private static void AddAssemblyToDomain(string assemblyFile)
    {
        if (!File.Exists(assemblyFile))
            return;

        try
        {
            byte[] assembly = File.ReadAllBytes(assemblyFile);
            Assembly asm = Assembly.Load(assembly);

            Instance.Logger.Log($"Loaded assembly \"{asm.GetName()}\" from \"{assemblyFile}\"");
        } catch (Exception ex)
        {
            Instance.Logger.LogError($"Failed to load assembly \"{assemblyFile}\" - {ex.TargetSite.Name}: \"{ex.Message}\"");
        }
    }
}
