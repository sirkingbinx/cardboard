using HarmonyLib;
using BepInEx;
using System.Reflection;

namespace Cardboard.Utils
{
    /// <summary>
    /// A class providing simple Harmony patching.
    /// </summary>
    public class CardboardHarmony
    {
        /// <summary>
        /// Patches the BaseUnityPlugin provided and returns the Harmony class used to patch the plugin.
        /// </summary>
        /// <param name="_instance">The BaseUnityPlugin to patch.</param>
        /// <returns>The Harmony instance that was used to patch it.</returns>
        public static Harmony PatchInstance(BaseUnityPlugin _instance)
        {
            Harmony thisHarmony = new Harmony(_instance.Info.Metadata.GUID);
            thisHarmony.PatchAll(Assembly.GetCallingAssembly());

            return thisHarmony;
        }

        /// <summary>
        /// Patches the assembly based on the GUID provided and returns the Harmony class used to patch the plugin.
        /// </summary>
        /// <param name="_instance">The mod GUID.</param>
        /// <returns>The Harmony instance that was used to patch it.</returns>
        public static Harmony PatchInstance(string GUID)
        {
            Harmony thisHarmony = new Harmony(GUID);
            thisHarmony.PatchAll(Assembly.GetCallingAssembly());
            
            return thisHarmony;
        }

        /// <summary>
        /// Unpatches the assembly of _instance.
        /// </summary>
        /// <param name="_instance">The patched Harmony instance.</param>
        public static void UnpatchInstance(Harmony _instance) => _instance.UnpatchSelf();
    }
}