using System;

using Chemistry_Tools.Core.Updaters;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class UpdateDownloadingViewModel : ViewModelBase
{
    private double _downloadProgress;
    private string? _downloadError;
    private bool _hasError;

    public bool HasError
    {
        get => _hasError;
        private set => this.RaiseAndSetIfChanged(ref _hasError, value);
    }

    public string? DownloadError
    {
        get => _downloadError;
        set
        {
            this.RaiseAndSetIfChanged(ref _downloadError, value);
            HasError = !string.IsNullOrEmpty(value);
        }
    }

    public double DownloadProgress
    {
        get => _downloadProgress;
        set => this.RaiseAndSetIfChanged(ref _downloadProgress, value);
    }

    public ReactiveCommand<bool, bool>? CancelDownloadCommand { get; }

    public UpdateDownloadingViewModel() => CancelDownloadCommand = ReactiveCommand.Create<bool, bool>(v => v);

    internal void ChangeProgress(float percentage)
        => DownloadProgress = percentage;

    internal void UpdateFinished(string path) => CancelDownloadCommand?.Execute(false);
    internal void DownloadHadError(UpdateItem item, Exception exception)
        => DownloadError = $"Error downloading {item.Title}\n{exception.Message}";
}
