using System.Reflection;
using System.Collections.Generic;

namespace Cardboard.Utilities
{
    /// <summary>
    /// A class providing simple Harmony patching.
    /// </summary>
    public static class CardboardHarmony
    {
        private static Dictionary<Assembly, HarmonyLib.Harmony> patchedInstances = new();

        /// <summary>
        /// Patches the assembly based on the GUID provided and returns the Harmony class used to patch the plugin.
        /// </summary>
        /// <param name="GUID">The mod GUID.</param>
        /// <returns>The Harmony instance that was used to patch it.</returns>
        public static HarmonyLib.Harmony PatchInstance(string GUID)
        {
            HarmonyLib.Harmony thisHarmony = new HarmonyLib.Harmony(GUID);
            thisHarmony.PatchAll(Assembly.GetCallingAssembly());

            return thisHarmony;
        }

        /// <summary>
        /// Patches the assembly based on the GUID and Assembly provided and returns the Harmony class used to patch the plugin.
        /// </summary>
        /// <param name="GUID">The mod GUID.</param>
        /// <param name="_asm">The assembly to patch.</param>
        /// <returns>The Harmony instance that was used to patch it.</returns>
        public static HarmonyLib.Harmony PatchInstance(string GUID, Assembly _asm)
        {
            HarmonyLib.Harmony thisHarmony = new HarmonyLib.Harmony(GUID);
            thisHarmony.PatchAll(_asm);

            return thisHarmony;
        }


        /// <summary>
        /// Unpatches the calling assembly. If you did not patch your plugin with CardboardLog, this will fail.
        /// </summary>
        public static void UnpatchInstance()
        {
            patchedInstances.TryGetValue(Assembly.GetCallingAssembly(), out HarmonyLib.Harmony patchedInstance);
            patchedInstance?.UnpatchSelf();
        }

        /// <summary>
        /// Unpatches the assembly of _instance.
        /// </summary>
        /// <param name="_instance">The patched Harmony instance.</param>
        public static void UnpatchInstance(HarmonyLib.Harmony _instance) => _instance.UnpatchSelf();
    }
}