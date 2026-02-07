using System.Collections.Generic;
using DMF_Lib;

namespace ExamplePlugin;



public class ExamplePlugin : IDuperyPlugin
{
    public ID PluginID { get; } = new ID("dugging", "ExamplePlugin"); 

    public Version PluginVersion { get; } = new Version(1, 0, 0);

    public IDictionary<ID, DependencyData> PluginDependencies { get; } = new Dictionary<ID, DependencyData>();

    public void PreInit(PreInitCore Core)
    {
        // Register config file or images here.
    }

    public void Init(InitCore Core)
    {
        // Register roles and other role data hee.

        // Register the new role "ExampleRole" with the framework and save the Roles value
        // for the actual role to use later.
        ExampleRole.roleValue = Core.RegisterRole(ExampleRole.ID, new RoleMetaData(
            new GRGLib.Maybe.Maybe<ID>(), // No image is register with this role (a default is used)
            RoleClassifications.INNOCENT,
            new ExampleRoleFactory()
        ));

        //Allow the example role to be (a starting role) generated in all regions.
        Core.RegisterRoleInRegion(ExampleRole.ID, true);
    }

    public void PostInit(PostInitCore Core) { }
}
