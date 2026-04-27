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

    internal GameObject CardboardModsObject;

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

        InitializeCardboardMods();

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

    private static void InitializeCardboardMods()
    {
        // Initialize CardboardMods

        LoadAvaliableAssemblies();

        Dictionary<string, Version> modVersions = new();
        Dictionary<string, (ModInfo, CardboardMod)> mods = new();
        List<CardboardMod> modComponents = new();

        foreach (var mod in GetInstancesOfTypeWithAttribute<ModInfo, CardboardMod>())
        {
            ModInfo modInfo = mod.Item1;
            CardboardMod cardboardMod = mod.Item2;

            if (modVersions.ContainsKey(modInfo.Uuid) && modVersions[modInfo.Uuid] > modInfo.Version)
            {
                Instance.Logger.Log($"Skipping initialization of [ {modInfo.Name} {modInfo.Version} ] because a newer version exists [ {modInfo.Name} {modVersions[modInfo.Uuid]} ]");
                continue;
            }
            else if (modVersions.ContainsKey(modInfo.Uuid) && modVersions[modInfo.Uuid] > modInfo.Version)
            {
                Instance.Logger.Log($"Replacing [ {modInfo.Name} {modInfo.Version} ] because a newer version is loaded [ {modInfo.Name} {modVersions[modInfo.Uuid]} ]");

                modVersions.Remove(modInfo.Uuid);
                mods.Remove(modInfo.Uuid);
                modComponents.Remove(mods[modInfo.Uuid].Item2);
            }

            cardboardMod.Info = modInfo;
            cardboardMod.ModLoader = Constants.Loader;

            modVersions.Add(modInfo.Uuid, modInfo.Version);
            mods.Add(modInfo.Uuid, (modInfo, cardboardMod));
            modComponents.Add(cardboardMod);
        }

        // Check for proper requirements for each mod & set initialization order

        foreach (var mod in mods.Values)
        {
            List<string> missingDependencies = [];
            CardboardRequirement[] requirements = [.. mod.Item2.GetType().GetCustomAttributes<CardboardRequirement>(false)];

            if (requirements.Length == 0)
            {
                modComponents.Move(modComponents.IndexOf(mod.Item2), 0);
            } else
            {
                int lastRequirementIndex = 0;

                foreach (CardboardRequirement requirement in requirements)
                {
                    if (!mods.ContainsKey(requirement.Uuid))
                    {
                        Instance.Logger.Log($"Skipping initialization of [ {mod.Item1.Uuid} {mod.Item1.Version} ] because it is missing a dependency [ {requirement.Uuid} ]");

                        modVersions.Remove(mod.Item1.Uuid);
                        mods.Remove(mod.Item1.Uuid);
                        modComponents.Remove(mod.Item2);
                    } else
                    {
                        CardboardMod requirementComp = mods[requirement.Uuid].Item2;
                        int idx = modComponents.IndexOf(requirementComp);
                        lastRequirementIndex = idx > lastRequirementIndex ? idx : lastRequirementIndex;
                    }
                }

                modComponents.Move(modComponents.IndexOf(mod.Item2), lastRequirementIndex + 1);
            }
        }

        // Load each of them

        Instance.CardboardModsObject = new GameObject($"Cardboard {Constants.Version} Bootstrapper");

        foreach (CardboardMod _mod in modComponents)
        {
            CardboardMod mod = (CardboardMod)Instance.CardboardModsObject.AddComponent(_mod.GetType());

            try { mod.OnBootstrapped(); }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}
