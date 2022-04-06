using System;
using System.Text.Json.Serialization;

using Avalonia.Media.Imaging;

using Chemistry_Tools.JSONConverters;
using Chemistry_Tools.UserSettings.WindowsResources;

namespace Chemistry_Tools.UserSettings;

public class Resources : IDisposable
{
    private bool _disposedValue;

    public MainWindowResources MainWindow { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                MainWindow.Dispose();
            }
            _disposedValue = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

[JsonConverter(typeof(BitmapConverter))]
public record ImageContainer(string Path, Bitmap Image);