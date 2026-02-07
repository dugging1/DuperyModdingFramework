using System;
using System.Collections.Generic;
using DMF_Lib;
using GRGLib.Maybe;
using Save;
using UnityEngine;
using Random = System.Random;

namespace ExamplePlugin;

public class ExampleRole : RoleData
{
    public static ID ID { get; } = new ID("ExamplePlugin", "ExampleRole");

    public ExampleRole(Random _random) : base(_random) { }

    public ExampleRole(Random _random, RoleData _true_self) : base(_random, _true_self) { }

    public ExampleRole(int _address, Vector2Int _board_position, Random _random, Maybe<RoleData> _true_self) : base(_address, _board_position, _random, _true_self) { }

    public ExampleRole(RoleDataSave _save, Maybe<RoleInfoSave> _info, Maybe<RoleDataSave> _disguise, Random _random, Func<RoleDataSave, RoleData> _from_save) : base(_save, _info, _disguise, _random, _from_save) { }


    public static Roles roleValue;
    public override Roles role => roleValue;

    public override string english_name => "Example Role";

    public override string english_description => "An example modded role.";

    public override List<KeywordInfo> keywords_used { get; } = new List<KeywordInfo>();

    public override bool hasInfo => false;

    public override string generateInfoString(CaseSnapshot _snap)
    {
        return "";
    }

    public override string getUniqueSaveData()
    {
        return "";
    }

    protected override Alignments getStartingAlignment()
    {
        return Alignments.GOOD;
    }

    protected override RoleClassifications getStartingClassification()
    {
        return RoleClassifications.INNOCENT;
    }

    // Has the same generation constraints as the Priest role.
    public override RoleGenerationConstraints getGenerationConstraints()
    {
        return new RoleGenerationConstraints(RoleGenerationMarkers.SELF_CONFIRM, new RoleTypeGenerationConstraint(ConstraintTypes.ALWAYS_AVOID, RoleGenerationMarkers.SELF_CONFIRM, DisguiseConstraintType.ALWAYS_IGNORE));
    }
}

public class ExampleRoleFactory : IRoleDataFactory
{
    public RoleData CreateEmpty()
    {
        return new ExampleRole(new Random());
    }

    public RoleData CreateSetRole(int address, (int, int) boardPos, Random rand, Maybe<RoleData> _true_self)
    {
        return new ExampleRole(address, new Vector2Int(boardPos.Item1, boardPos.Item2), rand, _true_self);
    }
}
