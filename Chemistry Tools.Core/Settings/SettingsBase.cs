using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Chemistry_Tools.Core.Settings;
public abstract class SettingsBase<T> : ISettings<T>, INotifyPropertyChanged
{
    readonly string _filePath;
    private readonly JsonSerializerOptions _options;

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
        File.Delete(_filePath);
        using var fileStream = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.Write);
        return _Save(fileStream);
    }

    protected abstract Task _Save(FileStream fileStream);

    protected void RaiseIfPropertyChanged<P>(ref P? backingField, P? newValue, [CallerMemberName] string? name = null, Action<P?>? doIfChanged = null)
        where P : IEquatable<P>
    {
        if (Equals(backingField, newValue))
            return;
        backingField = newValue;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        doIfChanged?.Invoke(newValue);
    }
}
