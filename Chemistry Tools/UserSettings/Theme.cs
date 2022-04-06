using System;

using Avalonia;
using Avalonia.Markup.Xaml;

using FluentAvalonia.Styling;

namespace Chemistry_Tools.UserSettings;
public class Theme : IEquatable<Theme>, IDisposable
{
    private bool _disposed;

    /// <summary>
    /// The name used for the theme. Must be unique.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The name of the theme this is based of.
    /// </summary>
    public string BaseTheme { get; set; }
    /// <summary>
    /// The directory in which the style file is placed.
    /// </summary>
    public string Directory { get; set; }
    /// <summary>
    /// The resources used by the theme.
    /// Things like icons and images should be here.
    /// </summary>
    public Resources Resources { get; set; }
    /// <summary>
    /// Compares two <see cref="Theme"/> objects.
    /// </summary>
    /// <param name="other">The other <see cref="Theme"/> instance.</param>
    /// <returns>True if the name is the same as this instance. False otherwise.</returns>
    public bool Equals(Theme? other)
    {
        if (other is null)
            return false;
        return Name == other.Name;
    }

    public void Apply()
    {
        var manager = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
        manager.RequestedTheme = BaseTheme;
    }

    public override string ToString() => Name;

    public override bool Equals(object obj) => Equals(obj as Theme);

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Resources.Dispose();
            }
            _disposed = true;
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
