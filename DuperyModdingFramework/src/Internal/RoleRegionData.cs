using System.Collections.Generic;
using DMF_Lib;

namespace DuperyModdingFramework.Internal;

public class RoleRegionData()
{
    public HashSet<ID> RegionAvailable { get; } = [];
    public HashSet<ID> RegionStartingAvailable { get; } = [];
    public bool IsGeneric { get; set; } = false;
    public bool IsGenericStarting { get; set; } = false;
}
