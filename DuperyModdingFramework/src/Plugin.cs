using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DMF_Lib;
using DMF_Lib.Helpers;
using DuperyModdingFramework.Internal;
using HarmonyLib;

namespace DuperyModdingFramework;

[BepInPlugin("dgging.DuperyModdingFramework", "Dupery Modding Framework", FrameworkVersion)]
[BepInProcess("Dupery.exe")]
public class DuperyModdingFramework : BaseUnityPlugin, PreInitCore, InitCore, PostInitCore
{
    public const string FrameworkVersion = "0.0.3";
    internal static new ManualLogSource Logger;
    internal MethodInfo getStateMethod;
    internal new FrameworkConfig Config = new(Path.GetFullPath("."));

    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Dupery Modding Framework loaded with version: {FrameworkVersion}");


        patchMethods();

        //SceneManager.sceneLoaded += OnSceneLoaded;
        getStateCallback();

        LoadPlugins();
    }

    private void patchMethods()
    {
        Harmony.CreateAndPatchAll(typeof(Patches));
    }

    private void getStateCallback()
    {
        getStateMethod = typeof(GameHandler).GetMembers()
                .Where(mem =>
                    mem is MethodInfo me && me.Name.Contains("get") &&
                    me.ReturnType.Equals(typeof(GameState)))
                .Select(mem => mem as MethodInfo)
                .First();
    }

    public GameState getState() => (GameState)getStateMethod.Invoke(null, null);

    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     Logger.LogInfo($"Loaded scene with name: {scene.name}");
    //     if (scene.name.Equals("CaseSelectionScene"))
    //     {
    //         Logger.LogInfo($"Processing CaseSelection Modifications");
    //         Logger.LogInfo($"Initial gamestate has {getState().available_roles.Count} roles.");
    //         //this.AddRegisteredRoles();
    //         //getState().available_roles.Add((Roles)Enum.GetValues(typeof(Roles)).Length);
    //         Logger.LogInfo($"Modified gamestate has {getState().available_roles.Count} roles.");
    //     }

    // }

    private void ListAllClasses()
    {
        Assembly asm = Assembly.GetAssembly(typeof(Accountant));

        using var file = File.OpenWrite("AssemblyData.txt");
        using var fw = new StreamWriter(file);

        foreach (var cls in asm.GetTypes())
        {
            string clsType = cls.IsClass ? "Class" :
                            cls.IsInterface ? "Interface" :
                            cls.IsEnum ? "Enum" : "Unknown";
            fw.WriteLine($"Type Name: {cls.Name} of kind: {clsType}");
            foreach (var mem in cls.GetMembers())
            {
                if (mem.DeclaringType.Equals(cls))
                {
                    if (mem is MethodInfo me)
                    {
                        string isStatic = me.IsStatic ? " (Static)" : "";
                        string args = "(" + me.GetParameters()
                                .Select(p => p.ParameterType.Name)
                                .Aggregate("", (l, r) => l + "," + r) + ")";
                        fw.WriteLine($"\tMethod Name{isStatic}: {mem.Name} with type: {args} -> {me.ReturnType}.");
                    }
                    else if (mem is FieldInfo f)
                    {
                        string isStatic = f.IsStatic ? " (Static)" : "";
                        fw.WriteLine($"\tField Name{isStatic}: {mem.Name} with type: {f.FieldType}.");
                    }
                    else if (mem is ConstructorInfo c)
                    {
                        string args = "(" + c.GetParameters()
                                .Select(p => p.ParameterType.Name)
                                .Aggregate("", (l, r) => l + "," + r) + ")";
                        fw.WriteLine($"\tConstructor with args: {args}.");
                    }
                    else if (mem is PropertyInfo p)
                    {
                        fw.WriteLine($"\tProperty Name: {mem.Name} with type: {p.PropertyType.Name}.");
                    }
                    else
                    {
                        //mem.GetUnderlyingType()
                        fw.WriteLine($"\tMember Name: {mem.Name} with type: {mem.GetType()}.");
                    }
                }
            }
        }

    }

    // public void AddRegisteredRoles()
    // {
    //     GameState gs = getState();
    //     int i = Enum.GetValues(typeof(Roles)).Length;
    //     foreach (var kv in RegistryRepo.RoleData.KeyValue)
    //     {
    //         RegistryRepo.RoleEnumValue.Register(kv.Key, i);
    //         gs.available_roles.Add((Roles)i);
    //         Logger.LogInfo($"Registered new Role with ID {kv.Key} and name {kv.Value.EnglishName} with Enum Value {i}");
    //     }
    // }

    public IDuperyPlugin getPluginCore(ID ID)
        => RegistryRepo.Plugins.Lookup(ID);

    public void RegisterImage(ID ID, string filePath)
    {
        RegistryRepo.Sprites.Register(ID, IMG2Sprite.LoadNewSprite(filePath));
    }

    public Roles RegisterRole(ID ID, RoleMetaData Role)
    {
        RegistryRepo.RoleData.Register(ID, Role);
        RegistryRepo.RoleRegions.Register(ID, new());
        RegistryRepo.RoleStartingRegions.Register(ID, new());
        int RoleVal = RegistryRepo.NextRoleEnumValue;
        RegistryRepo.NextRoleEnumValue++;
        RegistryRepo.RoleEnumValue.Register(ID, RoleVal);
        Logger.LogInfo($"Registered new Role with ID {ID} and with Enum Value {RoleVal}");
        return (Roles)RoleVal;
    }

    public void RegisterRoleInRegion(ID ID, bool starting_available_roles = false)
    {
        RegistryRepo.RoleRegions.Lookup(ID).Add(Regions.NONE);
        if (starting_available_roles)
            RegistryRepo.RoleStartingRegions.Lookup(ID).Add(Regions.NONE);
    }

    public void RegisterRoleInRegion(ID ID, Regions Region, bool starting_available_roles = false)
    {
        RegistryRepo.RoleRegions.Lookup(ID).Add(Region);
        if (starting_available_roles)
            RegistryRepo.RoleStartingRegions.Lookup(ID).Add(Region);
    }

    public IConfigHandler RegisterConfigFile(ID ID)
    {
        string fileName = $"{ID.Name}-{ID.Stem}.config";
        ConfigHandler conf = new(Config.ModsConfigFolderPath, fileName);
        RegistryRepo.ConfigFiles.Register(ID, conf);
        return conf;
    }


    void LoadPlugins()
    {
        IEnumerable<IDuperyPlugin> plugins = GetDuperyPlugins(GetPluginAssemblies(Config));
        if (!PluginDepsSatisfied(plugins))
        {
            // TODO: Good error message.
            throw new Exception("Plugin is missing dependencies.");
        }
        Logger.LogInfo($"Loaded {plugins.Count()} with all dependencies satisfied.");
        if (plugins.Count() == 0)
        {
            Logger.LogInfo("No plugins loaded. Ending initialisation process.");
            return;
        }
        Logger.LogInfo("Starting Dependency sorting.");
        List<IDuperyPlugin> sortedPlugins = SortByDependencies(plugins);
        Logger.LogInfo("Starting plugin Pre-Initialisation.");
        foreach (var p in sortedPlugins)
        {
            p.PreInit(this);
        }
        Logger.LogInfo("Starting plugin Initialisation.");
        foreach (var p in sortedPlugins)
        {
            p.Init(this);
        }
        Logger.LogInfo("Starting plugin Post-Initialisation.");
        foreach (var p in sortedPlugins)
        {
            p.PostInit(this);
        }
        Logger.LogInfo("Finished loading plugins.");
    }

    static IEnumerable<Assembly> GetPluginAssemblies(FrameworkConfig Config)
    {
        Logger.LogInfo($"Loading mods from {Config.ModsFolderPath}");
        if (Directory.Exists(Config.ModsFolderPath))
        {
            foreach (var f in Directory.EnumerateFiles(Config.ModsFolderPath, "*", SearchOption.AllDirectories))
            {
                Logger.LogInfo($"Checking file at: {f} with extension: {Path.GetExtension(f)}");
                if (Path.GetExtension(f).Equals(".dll"))
                {
                    Logger.LogInfo("Found dll file.");
                    yield return Assembly.LoadFile(Path.GetFullPath(f));
                }
            }
        }
    }

    static IEnumerable<IDuperyPlugin> GetDuperyPlugins(IEnumerable<Assembly> assemblies)
        => assemblies.SelectMany(a =>
            a.GetTypes().Where(
                t => t.IsClass
                    && !t.IsAbstract
                    && typeof(IDuperyPlugin).IsAssignableFrom(t)
            )
        .Select(cls => (IDuperyPlugin)Activator.CreateInstance(cls)));

    static bool PluginDepsSatisfied(IEnumerable<IDuperyPlugin> plugins)
    {
        Dictionary<ID, IDuperyPlugin> ps = plugins.ToDictionary(p => p.PluginID, p => p);
        foreach (var p in plugins)
        {
            foreach (var dep in p.PluginDependencies)
            {
                if (dep.Value.Flag == DependencyFlag.Required)
                {
                    if (!ps.ContainsKey(dep.Key) || !dep.Value.RequiredVersion.Contains(ps[dep.Key].PluginVersion))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    static List<IDuperyPlugin> SortByDependencies(IEnumerable<IDuperyPlugin> plugins)
        => TopologicalSort([.. plugins], [.. GetPluginDependencyEdges(plugins)], new DependencySorter());
    
    static IEnumerable<Tuple<IDuperyPlugin, IDuperyPlugin>> GetPluginDependencyEdges(IEnumerable<IDuperyPlugin> plugins)
    {
        foreach (var p in plugins)
        {
            foreach (var dep in p.PluginDependencies)
            {
                if (RegistryRepo.Plugins.ContainsKey(dep.Key))
                {
                    yield return new(RegistryRepo.Plugins.Lookup(dep.Key), p);
                }
            }
        }
    }

    // Adapted from: https://gist.github.com/Sup3rc4l1fr4g1l1571c3xp14l1d0c10u5/3341dba6a53d7171fe3397d13d00ee3f
    /// <summary>
    /// Topological Sorting (Kahn's algorithm) 
    /// </summary>
    /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="nodes">All nodes of directed acyclic graph.</param>
    /// <param name="edges">All edges of directed acyclic graph.</param>
    /// <returns>Sorted node in topological order.</returns>
    internal static List<T> TopologicalSort<T>(HashSet<T> nodes, HashSet<Tuple<T, T>> edges, IEqualityComparer<T> eq)
    {
        // Empty list that will contain the sorted elements
        var L = new List<T>();

        // Set of all nodes with no incoming edges
        var S = new HashSet<T>(nodes.Where(n => edges.All(e => !eq.Equals(e.Item2, n))));

        // while S is non-empty do
        while (S.Any())
        {

            //  remove a node n from S
            var n = S.First();
            S.Remove(n);

            // add n to tail of L
            L.Add(n);

            // for each node m with an edge e from n to m do
            foreach (var e in edges.Where(e => eq.Equals(e.Item1, n)).ToList())
            {
                var m = e.Item2;

                // remove edge e from the graph
                edges.Remove(e);

                // if m has no other incoming edges then
                if (edges.All(me => !eq.Equals(me.Item2, m)))
                {
                    // insert m into S
                    S.Add(m);
                }
            }
        }

        // if graph has edges then
        if (edges.Any())
        {
            // return error (graph has at least one cycle)
            throw new ArgumentException("Input graph contains a cycle.");
        }
        else
        {
            // return L (a topologically sorted order)
            return L;
        }
    }

    internal class DependencySorter : IEqualityComparer<IDuperyPlugin>
    {
        public bool Equals(IDuperyPlugin x, IDuperyPlugin y)
            => x.PluginID.Equals(y.PluginID);

        public int GetHashCode(IDuperyPlugin obj)
            => obj.GetHashCode();
    }
}
