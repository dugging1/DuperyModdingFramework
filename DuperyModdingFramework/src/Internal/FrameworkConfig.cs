using System.IO;
using GRGLib.Maybe;

namespace DuperyModdingFramework.Internal;

internal class FrameworkConfig : ConfigHandler
{

    internal FrameworkConfig(string BasePath, string configFileName = "DuperyModdingFramework.config")
        : base(BasePath, configFileName) { }

    internal string ModsFolderPath
    {
        get => Lookup<string>("General", "ModsFolderPath").returnValueOrDefault("Mods");
        set => Alter("General", "ModsFolderPath", value);
    }

    internal string ModsConfigFolderPath
    {
        get => Lookup<string>("General", "ModsConfigFolderPath").returnValueOrDefault("Config");
        set => Alter("General", "ModsConfigFolderPath", value);
    }
}

/*

DuperyModdingFramework.config


[Gernal]

# The folder (relative to Dupery.exe, or absolute) where the framework should look for mod dlls.
# The framework also searches sub-directories.
ModsFolderPath = "Mods"

# The folder (relative to Dupery.exe, or absolute) where the framework should look for/put mod config files.
ModsConfigFolderPath = "Config"



*/
