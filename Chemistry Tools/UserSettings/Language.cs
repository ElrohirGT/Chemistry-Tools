using System;

namespace Chemistry_Tools.UserSettings;

public class Language : IEquatable<Language>
{
    public string Name { get; set; }
    public string ThemeConfigurationLabel { get; set; }
    public string LanguageConfigurationLabel { get; set; }

    public bool Equals(Language? other)
    {
        if (other is null)
            return false;
        return Name == other.Name;
    }

    public override string ToString() => Name;

    public override bool Equals(object obj) => Equals(obj as Language);
}