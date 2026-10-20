using BepInEx;
using BepInEx.Bootstrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cardboard;

/// <summary>
/// Utilities for .NET reflection.
/// </summary>
public static class CardboardReflection
{
    /// <summary>
    /// Get all instances of classes that implement interface T.
    /// </summary>
    /// <typeparam name="T">The interface to search for.</typeparam>
    /// <returns>A list of activated instances of the interface.</returns>
    public static List<T> GetInstancesOfInterface<T>()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        var typeInstances = new List<T>();

        foreach (var type in types) {
            if (Activator.CreateInstance(type) is not T typeInstance)
                continue;

            typeInstances.Add(typeInstance);
        }

        return typeInstances;
    }

    /// <summary>
    /// Get all instances of classes that implement interface T, but limited to BepInEx plugins.
    /// </summary>
    /// <typeparam name="T">The interface to search for.</typeparam>
    /// <returns>A list of activated instances of the interface and their respective PluginInfos.</returns>
    public static List<(BepInEx.PluginInfo, T)> GetInstancesOfBepInterface<T>()
    {
        var infos = BepInExInfosToAssemblies;
        var assemblies = infos.Select(i => i.Item2).Distinct();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        var typeInstances = new List<(BepInEx.PluginInfo, T)>();

        foreach (var type in types)
        {
            if (Activator.CreateInstance(type) is not T typeInstance)
                continue;

            var owner = infos.First(i => i.Item2 == type.Assembly).Item1;
            typeInstances.Add((owner, typeInstance));
        }

        return typeInstances;
    }

    /// <summary>
    /// Get all instances of a type which has the specified attribute applied.
    /// </summary>
    /// <typeparam name="T1">The attribute to search for.</typeparam>
    /// <typeparam name="T2">The type to search for.</typeparam>
    /// <returns>A list of tuples (T1, T2) for each type and attribute.</returns>
    public static List<(T1, T2)> GetInstancesOfTypeWithAttribute<T1, T2>() where T1 : Attribute
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T2).IsAssignableFrom(type) && type.GetCustomAttributes<T1>(true).Any() && !type.IsInterface && !type.IsAbstract);

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

    /// <summary>
    /// All assemblies that have been loaded by BepInEx. 
    /// </summary>
    public static Assembly[] BepInExAssemblies
    {
        get
        {
            if (field is null || field.Any())
                field ??= Chainloader.PluginInfos.Values.Where(v => v.Instance != null).Select(v => v.Instance.GetType().Assembly).ToArray();

            return field;
        }
    }

    /// <summary>
    /// All assemblies that have been loaded by BepInEx accessible through their PluginInfo.
    /// </summary>
    public static List<(BepInEx.PluginInfo, Assembly)> BepInExInfosToAssemblies
    {
        get
        {
            if (field is null || field.Any())
                field ??= [.. Chainloader.PluginInfos.Values.Where(v => v.Instance != null).Select(v => (v, v.Instance.GetType().Assembly))];

            return field;
        }
    }
}