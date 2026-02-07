using GRGLib.Maybe;

namespace DMF_Lib;

/// <summary>
/// A struct collecting all required meta-data about a Dupery Role.
/// </summary>
/// <param name="SpriteID">The ID of the image (registered with the framework) for this role.</param>
/// <param name="StartingClassification">The default starting classification of this role.</param>
/// <param name="RoleFactory">A factory for producing instances of the new role.</param>
public record struct RoleMetaData(
    Maybe<ID> SpriteID,
    RoleClassifications StartingClassification,
    IRoleDataFactory RoleFactory
);
