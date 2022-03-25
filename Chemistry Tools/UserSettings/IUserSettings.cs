using System.ComponentModel;

using Chemistry_Tools.Core.Settings;

namespace Chemistry_Tools.UserSettings;
public interface IUserSettings : ISettings<IUserSettings>, INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets the current theme of the application.
    /// Implementors must fire an event if the property changed.
    /// </summary>
    Theme? CurrentTheme { get; set; }
    /// <summary>
    /// Gets or sets the current language of the application.
    /// Implementors must fire an event if the property changed.
    /// </summary>
    Language? CurrentLanguage { get; set; }

    /// <summary>
    /// Gets all the themes avaliable to the application.
    /// </summary>
    /// <returns>An array with all the themes available.</returns>
    Theme[] GetThemes();
    /// <summary>
    /// Gets all the languages available to the application.
    /// </summary>
    /// <returns>An array with all the languages available.</returns>
    Language[] GetLanguages();
}
