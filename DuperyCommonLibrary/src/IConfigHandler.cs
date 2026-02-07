using System;
using GRGLib.Maybe;

namespace DMF_Lib;

public interface IConfigHandler : IDisposable
{
    /// <summary>
    /// Lookup the value of an option under the given header.
    /// </summary>
    /// <typeparam name="T">The expected type of the value.</typeparam>
    /// <param name="Table">The name of the header in the config file.</param>
    /// <param name="Option">The name of the option.</param>
    /// <returns>The value of the option or nothing if it does not exist.</returns>
    Maybe<T> Lookup<T>(string Table, string Option);
    /// <summary>
    /// Lookup the value of a global option (not under any header).
    /// </summary>
    /// <typeparam name="T">The expected type of the value.</typeparam>
    /// <param name="Option">The name of the option.</param>
    /// <returns>The value of the option or nothing if it does not exist.</returns>
    Maybe<T> Lookup<T>(string Option);
    /// <summary>
    /// Add or change an option under the given header to the given value.
    /// This method does not write the change to file.
    /// </summary>
    /// <typeparam name="T">The type ofthe value.</typeparam>
    /// <param name="Table">The name of the header in the config file.</param>
    /// <param name="Option">The name of the option.</param>
    /// <param name="Value">The value to set the option to.</param>
    void Alter<T>(string Table, string Option, T Value);
    /// <summary>
    /// Add or change a global option to the given value.
    /// This method does not write the change to file.
    /// </summary>
    /// <typeparam name="T">The type ofthe value.</typeparam>
    /// <param name="Option">The name of the option.</param>
    /// <param name="Value">The value to set the option to.</param>
    void Alter<T>(string Option, T Value);
    /// <summary>
    /// Overwrites the current settings with those in the config file.
    /// If the config file does not exist, nothing changes.
    /// </summary>
    /// <returns>Whether the current setting have been overwritten (does not guarantee change in value).</returns>
    bool ReloadFile();
    /// <summary>
    /// Write the current settings to file.
    /// If the config file does not exist, it is created.
    /// </summary>
    void WriteToFile();
}
