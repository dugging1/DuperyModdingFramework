using System;
using System.Collections.Generic;
using DMF_Lib;
using UnityEngine;

namespace DuperyModdingFramework.Internal;

internal static class RegistryRepo
{
    internal static Registry<IDuperyPlugin> Plugins = new();
    internal static Registry<RoleMetaData> RoleData = new();
    internal static int NextRoleEnumValue = Enum.GetValues(typeof(Roles)).Length;
    internal static IntRegistry RoleEnumValue = new();
    // When a role is registered with RoleData, an empty hashset should be added here.
    internal static Registry<HashSet<Regions>> RoleRegions = new();
    internal static Registry<HashSet<Regions>> RoleStartingRegions = new();
    internal static Registry<Sprite> Sprites = new();
    internal static Registry<IConfigHandler> ConfigFiles = new();
}
