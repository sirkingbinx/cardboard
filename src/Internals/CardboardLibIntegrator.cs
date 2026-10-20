using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Bootstrap;
using GorillaGameModes;
using UnityEngine;

namespace Cardboard.Internals;

// Handles integration with Utilla/GL
internal static class CardboardLibIntegrator
{
    private const string Utilla = "org.legoandmars.gorillatag.utilla";
    private const string GorillaLibrary = "dev.gorillalibrary";
    private const string Translatilla = "dev.sirkingbinx.translatilla";

    private const BindingFlags Flags = BindingFlags.Public |
        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    internal static bool HasProvider => Chainloader.PluginInfos.ContainsKey(Utilla) ||
        Chainloader.PluginInfos.ContainsKey(GorillaLibrary);

    public static string Register(
        BaseUnityPlugin owner,
        string id,
        string name,
        GameModeType baseMode,
        Action<string> onJoin,
        Action<string> onLeave)
    {
        if (owner == null)
            throw new ArgumentNullException(nameof(owner));
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("A mode ID is required.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A mode name is required.", nameof(name));

        Assembly assembly = FindProvider();
        if (assembly == null)
            return null;

        bool utilla = assembly.GetType("Utilla.Behaviours.GamemodeManager") != null;
        string root = utilla ? "Utilla" : "GorillaLibrary";
        Type managerType = assembly.GetType(root + ".Behaviours." +
            (utilla ? "GamemodeManager" : "GameModeManager"), true);

        object manager = Get(managerType, null, "Instance");
        object initialization = Get(managerType, null, "Initialization");
        var task = initialization == null ? null :
            Get(initialization.GetType(), initialization, "Task") as Task;

        if (manager == null || task == null ||
            task.Status != TaskStatus.RanToCompletion)
            return null;

        if (NetworkSystem.Instance == null)
            return null;
        if (NetworkSystem.Instance.InRoom)
            throw new InvalidOperationException("Leave the room before registering a mode.");

        Type modeType = assembly.GetType(root + ".Models." +
            (utilla ? "Gamemode" : "GameModeWrapper"), true);
        Type entryType = assembly.GetType(root + ".PluginInfo", true);
        Type selectorType = assembly.GetType(root + ".Behaviours." +
            (utilla ? "UtillaGamemodeSelector" : "GameModeSelector"), true);

        var modes = (IList)Get(managerType, manager, "Gamemodes");
        var customModes = (IList)Get(managerType, manager, "CustomGameModes");
        var entries = (IList)Get(managerType, manager, "pluginInfos");

        ConstructorInfo constructor = modeType.GetConstructor(new[]
        {
            typeof(string), typeof(string), typeof(GameModeType?)
        });
        if (constructor == null)
            throw new MissingMethodException(modeType.FullName, ".ctor");

        MethodInfo addPrefab = Method(managerType,
            "AddGamemodeToPrefabPool", modeType);

        object mode = constructor.Invoke(new object[]
        {
            id, name, (GameModeType?)baseMode
        });
        string registeredId = (string)Get(modeType, mode, "ID");

        foreach (object existing in modes)
        {
            if (string.Equals((string)Get(modeType, existing, "ID"),
                registeredId, StringComparison.Ordinal))
                throw new InvalidOperationException("Mode already registered: " + registeredId);
        }

        if (GameMode.gameModeKeyByName.ContainsKey(registeredId))
            throw new InvalidOperationException("Mode ID is already in use: " + registeredId);
        if (!GameMode.gameModeKeyByName.ContainsKey(baseMode.ToString()))
            throw new InvalidOperationException("Base mode is not registered: " + baseMode);

        Array supportedModes = Array.CreateInstance(modeType, 1);
        supportedModes.SetValue(mode, 0);

        object entry = Activator.CreateInstance(entryType);
        Set(entryType, entry, "Plugin", owner);
        Set(entryType, entry, "Gamemodes", supportedModes);
        Set(entryType, entry, "OnGamemodeJoin", onJoin);
        Set(entryType, entry, "OnGamemodeLeave", onLeave);

        // Base-mode registration only adds an alias to the game's lookup
        bool added = false;
        try
        {
            addPrefab.Invoke(manager, new[] { mode });
            if (!GameMode.gameModeKeyByName.ContainsKey(registeredId))
                throw new InvalidOperationException("Provider did not register " + registeredId);

            modes.Add(mode);
            customModes.Add(mode);
            entries.Add(entry);
            added = true;
        }
        finally
        {
            if (!added)
            {
                modes.Remove(mode);
                customModes.Remove(mode);
                entries.Remove(entry);
                GameMode.gameModeKeyByName.Remove(registeredId);
            }
        }

        // Registration has succeeded even if a selector cannot redraw yet
        foreach (UnityEngine.Object selector in Resources.FindObjectsOfTypeAll(selectorType))
        {
            try
            {
                RefreshSelector(selectorType, selector);
            }
            catch (Exception error)
            {
                Debug.LogWarning("Could not refresh a mode selector: " + error);
            }
        }

        return registeredId;
    }

    private static Assembly FindProvider()
    {
        Assembly utilla = PluginAssembly(Utilla);
        Assembly library = PluginAssembly(GorillaLibrary);
        Assembly translator = PluginAssembly(Translatilla);

        if (translator != null)
        {
            Type plugin = translator.GetType("Translatilla.Plugin.Plugin", true);
            object setting = Get(plugin, null, "masterLibrary");
            if (setting == null)
                return null;

            string master = Get(setting.GetType(), setting, "Value")?.ToString();
            switch (master)
            {
                case "Utilla":
                    return utilla;
                case "GorillaLibrary":
                    return library;
                default:
                    throw new InvalidOperationException("Unknown Translatilla master: " + master);
            }
        }

        if (utilla != null && library != null)
            throw new InvalidOperationException(
                "Both libraries are loaded without Translatilla. Cannot choose a master library");

        return utilla ?? library;
    }

    private static Assembly PluginAssembly(string guid)
    {
        return Chainloader.PluginInfos.TryGetValue(guid, out var plugin) &&
            plugin.Instance != null ? plugin.Instance.GetType().Assembly : null;
    }

    private static void RefreshSelector(Type type, object selector)
    {
        ((IDictionary)Get(type, selector, "SelectorGameModes")).Clear();

        int capacity = (int)Get(type, selector, "PageCapacity");
        if (capacity <= 0 || Get(type, selector, "Layout") == null)
            return;

        var modes = (IList)Method(type, "GetSelectorGameModes").Invoke(selector, null);
        Set(type, selector, "PageCount", Math.Max(1, (modes.Count + capacity - 1) / capacity));
        Set(type, selector, "CurrentPage", 0);
        Method(type, "ShowPage").Invoke(selector, null);
    }

    private static MethodInfo Method(Type type, string name, params Type[] parameters)
    {
        return type.GetMethod(name, Flags, null, parameters, null)
            ?? throw new MissingMethodException(type.FullName, name);
    }

    private static object Get(Type type, object target, string name)
    {
        PropertyInfo property = type.GetProperty(name, Flags);
        if (property != null)
            return property.GetValue(target, null);

        FieldInfo field = type.GetField(name, Flags);
        if (field != null)
            return field.GetValue(target);

        throw new MissingMemberException(type.FullName, name);
    }

    private static void Set(Type type, object target, string name, object value)
    {
        PropertyInfo property = type.GetProperty(name, Flags);
        if (property != null)
        {
            property.SetValue(target, value, null);
            return;
        }

        FieldInfo field = type.GetField(name, Flags);
        if (field == null)
            throw new MissingMemberException(type.FullName, name);
        field.SetValue(target, value);
    }
}
