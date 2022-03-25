using System.ComponentModel;

using Avalonia.Logging;

using Chemistry_Tools.Core.Settings;

namespace Chemistry_Tools.AppSettings;

internal interface IAppSettings : ISettings<IAppSettings>, INotifyPropertyChanged
{
    /// <summary>
    /// Determines how much should be logged to Trace.
    /// </summary>
    LogEventLevel LogLevel { get; set; }
}