using System;
using GRGLib.Maybe;

namespace DMF_Lib;

public interface IRoleDataFactory
{
    RoleData CreateEmpty();
    RoleData CreateSetRole(int address, (int, int) boardPos, Random rand, Maybe<RoleData> _true_self);

}
