namespace DMF_Lib;

public interface PreInitCore
{
    /// <summary>
    /// Get the plugin instance with the given ID.
    /// Only the plugins that have been listed as dependencies are guarenteed to exist.
    /// They are also guarenteed to have finished pre-initialisation.
    /// </summary>
    /// <param name="ID">The ID of the desired plugin.</param>
    /// <returns>The plugin instance.</returns>
    IDuperyPlugin getPluginCore(ID ID);

    /// <summary>
    /// Registers a config file with the plugin.
    /// Uses the given ID as the file name for the config file.
    /// If a config file of that name exists, it is loaded from file.
    /// </summary>
    /// <param name="ID">The ID to register the config file with.</param>
    /// <returns>A handler for reading and writing configuration values.</returns>
    IConfigHandler RegisterConfigFile(ID ID);

    /// <summary>
    /// Load and register the image at the given filepath.
    /// The given ID will be used as the image's handle.
    /// </summary>
    /// <param name="ID">The ID to assign the loaded image.</param>
    /// <param name="filePath">The filepath to the image to load and register.</param>
    void RegisterImage(ID ID, string filePath);
}

public interface InitCore
{
    /// <summary>
    /// Get the plugin instance with the given ID.
    /// All plugins are guarenteed to have completed pre-initialisation.
    /// Only plugins listed as dependencies are guaranteed to have finished initialisation.
    /// </summary>
    /// <param name="ID">The ID of the desired plugin.</param>
    /// <returns>The plugin instance.</returns>
    IDuperyPlugin getPluginCore(ID ID);

    /// <summary>
    /// Registers a new modded role with framework.
    /// </summary>
    /// <param name="ID">The new role's ID.</param>
    /// <param name="Role">The metadata associated with the new role.</param>
    /// <returns>The assigned 'Roles' value for the new role.</returns>
    Roles RegisterRole(ID ID, RoleMetaData Role);

    /// <summary>
    /// Register a role as a generic role, which will appear in all regions.
    /// </summary>
    /// <param name="ID">The ID of the role.</param>
    /// <param name="startingRole">Whether the role should be available for generation from the start.</param>
    void RegisterRoleInRegion(ID ID, bool startingRole = false);

    /// <summary>
    /// Register a role to a specific region.
    /// </summary>
    /// <param name="ID">The ID of the role to register.</param>
    /// <param name="Region">The region to register the role to.</param>
    /// <param name="startingRole">Whether the role should be available for generation from the start.</param>
    void RegisterRoleInRegion(ID ID, Regions Region, bool startingRole = false);
}

public interface PostInitCore
{
    /// <summary>
    /// Get the plugin instance with the given ID.
    /// All plugins are guarenteed to have completed initialisation.
    /// Only plugins listed as dependencies are guaranteed to have finished post-initialisation.
    /// </summary>
    /// <param name="ID">The ID of the desired plugin.</param>
    /// <returns>The plugin instance.</returns>
    IDuperyPlugin getPluginCore(ID ID);
}
