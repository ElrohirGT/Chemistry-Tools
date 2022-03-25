
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Avalonia.Logging;

using Chemistry_Tools.Core.Settings;

namespace Chemistry_Tools.AppSettings;
public class AppSettings : SettingsBase<AppSettings>, IAppSettings
{
    readonly static JsonSerializerOptions OPTIONS = new()
    {
        WriteIndented = true
    };
    private const string CONFIG_FILE_PATH = "appconfig.json";

    public AppSettings() : base(CONFIG_FILE_PATH, OPTIONS)
    {
    }
    /// <inheritdoc/>
    public LogEventLevel LogLevel { get; set; }

    protected override Task _Save(FileStream fileStream) => JsonSerializer.SerializeAsync(fileStream, this, OPTIONS);
    IAppSettings? ISettings<IAppSettings>.Parse() => Parse();
}
