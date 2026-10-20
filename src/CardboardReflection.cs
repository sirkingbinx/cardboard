using BepInEx.Bootstrap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public static T[] GetInstancesOfInterface<T>() =>
        GetInstancesOfInterface<T>(AppDomain.CurrentDomain.GetAssemblies());

    /// <summary>
    /// Get all instances of classes that implement interface T.
    /// </summary>
    /// <typeparam name="T">The interface to search for.</typeparam>
    /// <param name="assemblyPool">The assemblies to search for the interface in.</param>
    /// <returns>A list of activated instances of the interface.</returns>
    public static T[] GetInstancesOfInterface<T>(IEnumerable<Assembly> assemblyPool)
    {
        var types = assemblyPool.SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        var typeInstances = new List<T>();

        foreach (var type in types) {
            if (Activator.CreateInstance(type) is not T typeInstance)
                continue;

            typeInstances.Add(typeInstance);
        }

        return typeInstances.ToArray();
    }

    /// <summary>
    /// Get all instances of a type which has the specified attribute applied.
    /// </summary>
    /// <typeparam name="T1">The attribute to search for.</typeparam>
    /// <typeparam name="T2">The type to search for.</typeparam>
    /// <returns>A list of tuples (T1, T2) for each type and attribute.</returns>
    public static (T1, T2)[] GetInstancesOfTypeWithAttribute<T1, T2>() where T1 : Attribute
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

        return typeInstances.ToArray();
    }
}