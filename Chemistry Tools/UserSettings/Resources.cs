using System.Text.Json.Serialization;

using Avalonia.Media.Imaging;

using Chemistry_Tools.JSONConverters;

namespace Chemistry_Tools.UserSettings;

public class Resources
{
    /// <summary>
    /// The icon xaml filename.
    /// </summary>
    public ImageContainer Icon { get; set; }
}

[JsonConverter(typeof(BitmapConverter))]
public record ImageContainer(string Path, Bitmap Image);