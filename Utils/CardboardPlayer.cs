namespace Cardboard.Utils
{
    public class Player
    {
        public static bool Steam { get; internal set; }
    }

    [HarmonyPatch(typeof(GorillaComputer), "Initialise")]
    class GetGamePlatform
    {
        static void Postfix() => Player.Steam = PlayFabAuthenticator.instance.platform.PlatformTag.ToLower().Contains("steam");
    }
}