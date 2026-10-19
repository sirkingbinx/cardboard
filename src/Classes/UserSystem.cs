using System;
using System.IO;

namespace Cardboard;

/// <summary>
/// Methods and values for system-specific methods. This is most commonly used with mods who wish to implement Linux over Wine support.
/// </summary>
public class UserSystem
{
    /// <summary>
    /// The current operating system that the player is using.
    /// </summary>
    public SystemEnvironment SystemEnvironment { get; internal set; }

    /// <summary>
    /// Gets the folder that is designated as a "user folder". For Windows, this would be C:\Users\[username], on Linux, this would be /home/[username].
    /// </summary>
    /// <returns>An absolute path to the user folder.</returns>
    public string GetHomeFolder()
    {
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (SystemEnvironment != SystemEnvironment.WindowsOverWine)
            return userProfile;
        
        string windowsUserName = new DirectoryInfo(userProfile).Name;
        string linuxHome = Path.Combine("Z:\\home", windowsUserName);

        return linuxHome;
    }
}