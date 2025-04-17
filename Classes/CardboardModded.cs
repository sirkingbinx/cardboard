using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Cardboard.Utils;

using Photon.Pun;
using GorillaNetworking;

namespace Cardboard.Classes
{
    /// <summary>
    /// You should use attributes [CardboardModdedHandler], [CardboardModdedJoin] and [CardboardModdedLeave] instead.
    /// </summary>
    public class CardboardModded
    {
        public static List<Assembly> Assemblies = new List<Assembly>();

        public static void CallModdedEvent(ModdedEventType mType)
        {
            List<MethodInfo> Attributes = new List<MethodInfo>();

            foreach (Assembly assembly in Assemblies)
            {
                Attributes.AddRange(mType == ModdedEventType.ModdedJoin
                                             ? Method.FindCaseOfAttribute<CardboardModdedJoin>(assembly)
                                             : Method.FindCaseOfAttribute<CardboardModdedLeave>(assembly)
                );
            }

            foreach (MethodInfo me in Attributes)
                Method.TryInvoke(me);
        }
    }

    /// <summary>
    /// The class attribute for providing modded functionality.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CardboardModdedHandler : Attribute
    {
        public CardboardModdedHandler() => CardboardModded.Assemblies.Add(Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// An attribute for methods to handle joining modded lobbies.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CardboardModdedJoin : Attribute { }

    /// <summary>
    /// An attribute for methods to handle leaving modded lobbies.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CardboardModdedLeave : Attribute { }
}