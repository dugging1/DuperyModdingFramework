namespace DMF_Lib;

public record struct ID(string Stem, string Name);


public static class BaseGameIDs
{
    public static ID RegionClock { get; } = new ID("Base", "RegionClock");
    public static ID RegionDocks { get; } = new ID("Base", "RegionDocks");
    public static ID RegionCasino { get; } = new ID("Base", "RegionCasino");
}
