using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class CardboardConfig
{
    public static void UpdateConfig()
    {
        ConfigFile cfg = new ConfigFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cardboard.cfg"), true);

        GamePlatform spoofing = cfg.Bind("General", "GamePlatformPatch", GamePlatform.None, "Replace the detected platform with one of your choice (Not recommended)");
        if (spoofing != GamePlatform.None)
            CardboardPlayer.Platform = spoofing;
    }
}