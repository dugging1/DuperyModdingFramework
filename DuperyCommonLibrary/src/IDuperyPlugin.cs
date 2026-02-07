using System.Collections.Generic;

namespace DMF_Lib;

public interface IDuperyPlugin
{
    /// <summary>
    /// The ID of the plugin. 
    /// This should be constant through execution and plugin version.
    /// This ID will be used by other plugins to reference this plugin.
    /// </summary>
    ID PluginID { get; }
    /// <summary>
    /// The current version of this plugin.
    /// This should not change through execution.
    /// </summary>
    Version PluginVersion { get; }
    /// <summary>
    /// A collection of both required and optional dependencies for this plugin.
    /// The key of this dicitiony is the ID of the plugin that is the dependency.
    /// </summary>
    IDictionary<ID, DependencyData> PluginDependencies { get; }

    /// <summary>
    /// The method run during the Pre-initialisation phase of mod loading.
    /// See the "PreInitCore" type for expected behaviour in this phase.
    /// </summary>
    /// <param name="Core">A reference to a core framework object that is capable of handling the required functionality for this phase.</param>
    void PreInit(PreInitCore Core);

    /// <summary>
    /// The method run during the initialisation phase of mod loading.
    /// See the "InitCore" type for expected behaviour in this phase.
    /// </summary>
    /// <param name="Core">A reference to a core framework object that is capable of handling the required functionality for this phase.</param>
    void Init(InitCore Core);

    /// <summary>
    /// The method run during the Post-initialisation phase of mod loading.
    /// See the "PostInitCore" type for expected behaviour in this phase.
    /// </summary>
    /// <param name="Core">A reference to a core framework object that is capable of handling the required functionality for this phase.</param>
    void PostInit(PostInitCore Core);
}
