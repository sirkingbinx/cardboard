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

    /// <inheritdoc cref="Environment.GetFolderPath(Environment.SpecialFolder)"/>
    /// <remarks>
    /// This substitute for Environment.GetFolderPath is designed to also work on Linux systems (WindowsOverWine).
    /// </remarks>
    public string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option = Environment.SpecialFolderOption.None)
    {
        if (SystemEnvironment == SystemEnvironment.Windows)
            return Environment.GetFolderPath(folder, option);

        string folderPath = string.Empty;

        switch (folder)
        {
            case Environment.SpecialFolder.UserProfile:
                folderPath = UserFolder;
                break;
            case Environment.SpecialFolder.Desktop:
                folderPath = Path.Combine(UserFolder, "Desktop");
                break;
            case Environment.SpecialFolder.MyDocuments:
                folderPath = Path.Combine(UserFolder, "Documents");
                break;
            case Environment.SpecialFolder.ApplicationData:
                folderPath = Path.Combine(UserFolder, ".config");
                break;
            case Environment.SpecialFolder.LocalApplicationData:
                folderPath = Path.Combine(UserFolder, ".local", "share");
                break;
            case Environment.SpecialFolder.CommonApplicationData:
                folderPath = Path.Combine(RootFolder, "usr", "share");
                break;
            case Environment.SpecialFolder.MyPictures:
                folderPath = Path.Combine(UserFolder, "Pictures");
                break;
            case Environment.SpecialFolder.MyMusic:
                folderPath = Path.Combine(UserFolder, "Music");
                break;
            case Environment.SpecialFolder.MyVideos:
                folderPath = Path.Combine(UserFolder, "Videos");
                break;
            case Environment.SpecialFolder.Templates:
                folderPath = Path.Combine(UserFolder, "Templates");
                break;
            case Environment.SpecialFolder.CommonTemplates:
                folderPath = Path.Combine(RootFolder, "usr", "share", "Templates");
                break;
            case Environment.SpecialFolder.Fonts:
                folderPath = Path.Combine(UserFolder, ".local", "share", "Fonts");
                break;
            default:
                return "";
        }

        if (string.IsNullOrEmpty(folderPath))
            return string.Empty;
        
        if (option == Environment.SpecialFolderOption.Create && !Directory.Exists(folderPath))
        {
            try { Directory.CreateDirectory(folderPath); }
            catch (UnauthorizedAccessException) { return string.Empty; }
        }

        return folderPath;
    }

    private string UserFolder {
        get
        {
            if (!string.IsNullOrEmpty(field))
                return field;

            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (SystemEnvironment != SystemEnvironment.Linux)
                return userProfile;

            field = Path.Combine("Z:\\home", new DirectoryInfo(userProfile).Name);

            return field;
        }
    }

    private string RootFolder {
        get
        {
            if (string.IsNullOrEmpty(field))
                field = SystemEnvironment == SystemEnvironment.Linux ? "Z:" : "C:";

            return field;
        }
    }
}