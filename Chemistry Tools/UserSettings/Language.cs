using System;

using Chemistry_Tools.UserSettings.WindowsLanguage;

namespace Chemistry_Tools.UserSettings;

public sealed class Language : IEquatable<Language>
{
    /// <summary>
    /// Represents the name of the language. This value must be unique.
    /// </summary>
    public string Name { get; set; }
    public ConfigurationWindowLanguage ConfigurationWindow { get; set; }
    public UpdatePopUpWindowLanguage UpdatePopUpWindow { get; set; }
    public UpdateDownloadingWindowLanguage UpdateDownloadingWindow { get; set; }
    public ErrorPopUpWindowLanguage ErrorPopUpWindow { get; set; }
    public override string ToString() => Name;
    public bool Equals(Language? other)
    {
        if (other is null)
            return false;
        return Name == other.Name;
    }
    public override bool Equals(object obj) => Equals(obj as Language);

    public override int GetHashCode() => Name.GetHashCode();
}