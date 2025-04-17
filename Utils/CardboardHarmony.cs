using HarmonyLib;
using BepInEx;

namespace Cardboard.Utils
{
    public class CardboardHarmony
    {
        public static Harmony PatchInstance(BepInPlugin _instance)
        {
            Harmony thisHarmony = new Harmony(_instance.Info.Metadata.GUID);
            thisHarmony.PatchAll(Assembly.GetExecutingAssembly());

            return thisHarmony;
        }

        public static Harmony PatchInstance(BepInPlugin _instance, Assembly _GetExecutingAssembly)
        {
            Harmony thisHarmony = new Harmony(_instance.Info.Metadata.GUID);
            thisHarmony.PatchAll(_GetExecutingAssembly);
            
            return thisHarmony;
        }

        public static Harmony PatchInstance(string GUID)
        {
            Harmony thisHarmony = new Harmony(_GUID);
            thisHarmony.PatchAll(_Assembly.GetExecutingAssembly());
            
            return thisHarmony;
        }

        public static Harmony PatchInstance(string GUID, Assembly _GetExecutingAssembly)
        {
            Harmony thisHarmony = new Harmony(_GUID);
            thisHarmony.PatchAll(_GetExecutingAssembly);
            
            return thisHarmony;
        }

        public static void UnpatchInstance(Harmony _instance) => _instance.UnpatchSelf();
    }
}