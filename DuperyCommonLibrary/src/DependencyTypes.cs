using GRGLib.Maybe;

namespace DMF_Lib;

/// <summary>
/// A collection of dependency information.
/// </summary>
/// <param name="RequiredVersion">The range of valid versions.</param>
/// <param name="Flag">Dependency is Required/Optional.</param>
public record struct DependencyData(VersionRange RequiredVersion, DependencyFlag Flag);


/// <summary>
/// A flag to denote wether a dependency is required or optional.
/// </summary>
public enum DependencyFlag
{
    Required,
    Optional
}

/// <summary>
/// A struct denoting a version following semantic versioning.
/// </summary>
public record struct Version(int Major, int Minor, int Build)
{
    public static bool operator <(Version l, Version r) =>
            l.Major < r.Major
        || (l.Major == r.Major && l.Minor < r.Minor)
        || (l.Major == r.Major && l.Minor == r.Minor && l.Build < r.Build);

    public static bool operator >(Version l, Version r) =>
            l.Major > r.Major
        || (l.Major == r.Major && l.Minor > r.Minor)
        || (l.Major == r.Major && l.Minor == r.Minor && l.Build > r.Build);

    public static bool operator <=(Version l, Version r) =>
        l < r || l == r;

    public static bool operator >=(Version l, Version r) =>
        l > r || l == r;
}

/// <summary>
/// A representation of a version range constraint.
/// A nothing in a given position means no constraints.
/// </summary>
/// <param name="Lower">The lowest valid version. Nothing means no lowest valid version.</param>
/// <param name="Upper">The highest valid version. Nothing means no highest valid version.</param>
public record struct VersionRange(Maybe<Version> Lower, Maybe<Version> Upper)
{
    /// <summary>
    /// Checks if the given version is inside the range.
    /// </summary>
    public bool Contains(Version v) =>
        Lower.fmap(l => l <= v).returnValueOrDefault(true)
        && Upper.fmap(u => v <= u).returnValueOrDefault(true);
}
