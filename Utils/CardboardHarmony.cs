using HarmonyLib;
using BepInEx;
using System.Reflection;
using System.Collections.Generic;

namespace Cardboard.Utils
{
    /// <summary>
    /// A class providing simple Harmony patching.
    /// </summary>
    public static class CardboardHarmony
    {
        private static Dictionary<Assembly, Harmony> patchedInstances = new Dictionary<Assembly, Harmony>();
        
        /// <summary>
        /// Patches the BaseUnityPlugin provided and returns the Harmony class used to patch the plugin.
        /// </summary>
        /// <param name="_instance">The BaseUnityPlugin to patch.</param>
        /// <returns>The Harmony instance that was used to patch it.</returns>
        public static Harmony PatchInstance(BaseUnityPlugin _instance)
        {
            Harmony thisHarmony = new Harmony(_instance.Info.Metadata.GUID);
            thisHarmony.PatchAll(Assembly.GetAssembly(_instance.GetType()));
            patchedInstances.Add(Assembly.GetAssembly(_instance.GetType()), thisHarmony);

            return thisHarmony;
        }

        /// <summary>
        /// Patches the assembly based on the GUID provided and returns the Harmony class used to patch the plugin.
        /// </summary>
        /// <param name="GUID">The mod GUID.</param>
        /// <returns>The Harmony instance that was used to patch it.</returns>
        public static Harmony PatchInstance(string GUID)
        {
            Harmony thisHarmony = new Harmony(GUID);
            thisHarmony.PatchAll(Assembly.GetCallingAssembly());

            return thisHarmony;
        }

        /// <summary>
        /// Patches the assembly based on the GUID and Assembly provided and returns the Harmony class used to patch the plugin.
        /// </summary>
        /// <param name="GUID">The mod GUID.</param>
        /// <param name="_asm">The assembly to patch.</param>
        /// <returns>The Harmony instance that was used to patch it.</returns>
        public static Harmony PatchInstance(string GUID, Assembly _asm)
        {
            Harmony thisHarmony = new Harmony(GUID);
            thisHarmony.PatchAll(_asm);

            return thisHarmony;
        }


        /// <summary>
        /// Unpatches the calling assembly. If you did not patch your plugin with CardboardLog, this will fail.
        /// </summary>
        public static void UnpatchInstance()
        {
            patchedInstances.TryGetValue(Assembly.GetCallingAssembly(), out Harmony patchedInstance);

            if (patchedInstance != null)
                patchedInstance.UnpatchSelf();
        }

        /// <summary>
        /// Unpatches the assembly of _instance.
        /// </summary>
        /// <param name="_instance">The patched Harmony instance.</param>
        public static void UnpatchInstance(Harmony _instance) => _instance.UnpatchSelf();
    }
}