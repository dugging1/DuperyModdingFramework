using System;
using System.IO;
using DMF_Lib;
using GRGLib.Maybe;
using Tomlyn;
using Tomlyn.Model;


namespace DuperyModdingFramework.Internal;

internal class ConfigHandler : IConfigHandler
{
    internal TomlTable config;
    internal string BasePath;
    internal string configFilePath;

    internal ConfigHandler(string BasePath, string configFileName)
    {
        this.BasePath = Path.GetFullPath(BasePath);
        configFilePath = Path.Combine(BasePath, configFileName);
        if (File.Exists(configFilePath))
        {
            try
            {
                config = Toml.ToModel(File.ReadAllText(configFilePath));
            }
            catch (Exception e)
            {
                
                DuperyModdingFramework.Logger.LogError($"An error occured when loading the framework config file:\n {e}");
                config = new();
            }
        }
        else
        {
            config = new TomlTable();
        }
    }

    public void Alter<T>(string Table, string Option, T Value)
    {
        if (!config.ContainsKey(Table))
        {
            config.Add(Table, new TomlTable());
        }
        if (config[Table] is TomlTable tt)
        {
            tt[Option] = Value;
        }
        else
        {
            throw new ArgumentException($"Table {Table} points to a non-table option in a config file({configFilePath}).");
        }
    }

    public void Alter<T>(string Option, T Value)
    {
        if (config[Option] is TomlTable)
        {
            throw new ArgumentException($"Table {Option} points to a table in a config file({configFilePath}).");
        }
        else
        {
            config[Option] = Value;
        }
    }

    public void Dispose()
    {
        string backupPath = Path.Combine(Path.GetDirectoryName(configFilePath), "config.backup");
        File.Copy(configFilePath, backupPath);
        WriteToFile();
    }

    public Maybe<T> Lookup<T>(string table, string option)
    {
        if (config.ContainsKey(table))
        {
            if (config[table] is TomlTable tt)
            {
                object obj = tt[option];
                if (obj is not null)
                {
                    return new Maybe<T>((T)obj);
                }
            }
        }
        return new Maybe<T>();
    }

    public Maybe<T> Lookup<T>(string Option)
    {
        if (config.ContainsKey(Option))
        {
            object obj = config[Option];
            if (obj is not null)
            {
                return new Maybe<T>((T)obj);
            }
        }
        return new Maybe<T>();
    }

    public bool ReloadFile()
    {
        if (File.Exists(configFilePath))
        {
            using var f = new StreamReader(File.OpenRead(configFilePath));
            config = Toml.ToModel(f.ReadToEnd());
            return true;
        }
        return false;
    }

    public void WriteToFile()
    {
        if (File.Exists(configFilePath))
        {
            File.Delete(configFilePath);
        }
        using var f = new StreamWriter(File.OpenWrite(configFilePath));
        f.Write(Toml.FromModel(config));
        f.Flush();
    }
}
