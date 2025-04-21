using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MTAssets.EasyMeshCombiner;

namespace Cardboard.Utils
{ 
    public class Method
    {
        /// <summary>
        /// Attempts to invoke the provided MethodInfo.
        /// </summary>
        /// <param name="_toInvoke">MethodInfo to try and invoke.</param>
        public static void TryInvoke<T>(MethodInfo _toInvoke)
        {
            try
            {
                _toInvoke.Invoke(typeof(T), null);
            }
            catch { };
        }

        /// <summary>
        /// Trys to find all methods using attribute T and returns any methods with it.
        /// </summary>
        /// <typeparam name="T">The associated attribute to search for.</typeparam>
        /// <returns>A list of all MethodInfo(s) found.</returns>
        public static List<MethodInfo> FindCaseOfAttribute<T>()
        {
            var methods = Assembly.GetCallingAssembly().GetTypes()
                                  .SelectMany(t => t.GetMethods())
                                  .Where(m => m.GetCustomAttributes(typeof(T), false).Length > 0)
                                  .ToArray();

            return new List<MethodInfo>(methods);
        }

        /// <summary>
        /// Trys to find all methods using attribute T and returns any methods with it.
        /// </summary>
        /// <typeparam name="T">The associated attribute to search for.</typeparam>
        /// <param name="assembly">The assembly to search in.</param>
        /// <returns>A list of all MethodInfo(s) found.</returns>
        public static List<MethodInfo> FindCaseOfAttribute<T>(Assembly assembly)
        {
            var methods = assembly.GetTypes()
                                  .SelectMany(t => t.GetMethods())
                                  .Where(m => m.GetCustomAttributes(typeof(T), false).Length > 0)
                                  .ToArray();

            return new List<MethodInfo>(methods);
        }
    }
}