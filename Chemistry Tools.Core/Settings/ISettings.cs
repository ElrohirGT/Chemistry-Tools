namespace Chemistry_Tools.Core.Settings;
public interface ISettings<T>
{
    /// <summary>
    /// Saves the current settings.
    /// </summary>
    /// <returns>A task that completes once the settings have been saved.</returns>
    Task Save();
    /// <summary>
    /// Parses the settings from the specified file.
    /// </summary>
    /// <returns>A task that completes once the settings have been parsed.</returns>
    T? Parse();
}
