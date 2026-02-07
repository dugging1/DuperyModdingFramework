using System.IO;
using GRGLib.Maybe;
using Tomlyn.Model;

namespace DuperyModdingFramework.Internal;

internal class FrameworkConfig : ConfigHandler
{

    internal FrameworkConfig(string BasePath, string configFileName = "DuperyModdingFramework.config")
        : base(BasePath, configFileName)
    {
        // config.Add("General", new TomlTable());
        // ((TomlTable)config["General"]).Add("ModsFolderPath", "Mods");
        // ((TomlTable)config["General"]).Add("ModsConfigFolderPath", "Config");
        // ((TomlTable)config["General"]).Add("ClearRegionAvailableRoles", false);
        // ((TomlTable)config["General"]).Add("ClearRegionStartingRoles", false);
    }

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

    internal bool ClearRegionAvailableRoles
    {
        get => Lookup<bool>("General", "ClearRegionAvailableRoles").returnValueOrDefault(false);
        set => Alter("General", "ClearRegionAvailableRoles", false);
    }

    internal bool ClearRegionStartingRoles
    {
        get => Lookup<bool>("General", "ClearRegionStartingRoles").returnValueOrDefault(false);
        set => Alter("General", "ClearRegionStartingRoles", false);
    }
}

/*

DuperyModdingFramework.config


[General]

# The folder (relative to Dupery.exe, or absolute) where the framework should look for mod dlls.
# The framework also searches sub-directories.
ModsFolderPath = "Mods"

# The folder (relative to Dupery.exe, or absolute) where the framework should look for/put mod config files.
ModsConfigFolderPath = "Config"

# Should the framework remove the vanilla list of roles for each region.
# Vanilla roles can be added back via the framework's region registry.
ClearRegionAvailableRoles = false

# Should the framework remove the vanilla list of starting roles for each region.
# Vanilla roles can be added back via the framework's region registry.
ClearRegionStartingRoles = false

*/
