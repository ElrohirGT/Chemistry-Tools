using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Chemistry_Tools.Core.Settings;
public abstract class SettingsBase<T> : ISettings<T>, INotifyPropertyChanged
{
    readonly string _filePath;
    private JsonSerializerOptions? _options = new() { WriteIndented = true };

    protected SettingsBase(string filePath, JsonSerializerOptions options)
    {
        _filePath = filePath;
        _options = options;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    /// <inheritdoc/>
    public T? Parse()
    {
        using var fileStream = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Read);
        return JsonSerializer.Deserialize<T>(fileStream, _options);
    }
    /// <inheritdoc/>
    public Task Save()
    {
        using var fileStream = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Write);
        return _Save(fileStream);
    }

    protected abstract Task _Save(FileStream fileStream);

    protected void RaiseIfPropertyChanged<T>(ref T? backingField, T? newValue, [CallerMemberName] string? name = null) where T : IEquatable<T>
    {
        if (Equals(backingField, newValue))
            return;
        backingField = newValue;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
