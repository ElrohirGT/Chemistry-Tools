using System;

namespace Chemistry_Tools.UserSettings.WindowsResources;
public class MainWindowResources : IDisposable
{
    private bool _disposedValue;

    /// <summary>
    /// The icon for the settings button that routes to the settings view.
    /// </summary>
    public ImageContainer SettingsIcon { get; set; }
    /// <summary>
    /// The icon for the home button that routes to the home view.
    /// </summary>
    public ImageContainer HomeIcon { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                SettingsIcon.Image.Dispose();
                HomeIcon.Image.Dispose();
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
