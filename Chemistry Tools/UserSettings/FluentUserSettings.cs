using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Chemistry_Tools.Core.Settings;

namespace Chemistry_Tools.UserSettings;
public class FluentUserSettings : SettingsBase<FluentUserSettings>, IUserSettings
{
    readonly static JsonSerializerOptions OPTIONS = new()
    {
        WriteIndented = true
    };
    private const string CONFIG_FILE_PATH = "user.json";
    private Language? _currentLanguage;
    private Theme? _currentTheme;

    public FluentUserSettings() : base(CONFIG_FILE_PATH, OPTIONS)
    {
    }
    /// <inheritdoc/>
    public Theme? CurrentTheme
    {
        get => _currentTheme;
        set
        {
            RaiseIfPropertyChanged(ref _currentTheme, value);
            _currentTheme?.Apply();
        }
    }
    /// <inheritdoc/>
    public Language? CurrentLanguage
    {
        get => _currentLanguage;
        set => RaiseIfPropertyChanged(ref _currentLanguage, value);
    }

    public Language[] GetLanguages() => ExtractFromFiles<Language>("langs");
    public Theme[] GetThemes() => ExtractFromFiles<Theme>("themes");

    private static T[] ExtractFromFiles<T>(string folder)
    {
        if (!Directory.Exists(folder))
            return Array.Empty<T>();

        var jsonFiles = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);
        var list = new List<T>();
        foreach (var file in jsonFiles)
        {
            using var fileStream = File.OpenRead(file);
            var deserializedObject = JsonSerializer.Deserialize<T>(fileStream);
            if (deserializedObject is not null)
                list.Add(deserializedObject);
        }

        return list.ToArray();
    }

    protected override Task _Save(FileStream fileStream) => JsonSerializer.SerializeAsync(fileStream, this, OPTIONS);
    IUserSettings? ISettings<IUserSettings>.Parse() => Parse();
}
