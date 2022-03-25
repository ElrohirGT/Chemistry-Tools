using System;

using Avalonia;
using Avalonia.Markup.Xaml;

using FluentAvalonia.Styling;

namespace Chemistry_Tools.UserSettings;
public class Theme : IEquatable<Theme>
{
    /// <summary>
    /// The name used for the theme.
    /// </summary>
    public string Name { get; set; }
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
        manager.RequestedTheme = Name;
        //string path = Path.Combine(Environment.CurrentDirectory, "themes", Directory, Resources.SubmitButtonIcon.Path);
        //try
        //{
        //    var iconStyle = AvaloniaRuntimeXamlLoader.Parse<Styles>(File.ReadAllText(path));
        //    Application.Current.Styles.Add(iconStyle);
        //}
        //catch (Exception e)
        //{
        //    throw;
        //}
    }

    public override string ToString() => Name;
}
