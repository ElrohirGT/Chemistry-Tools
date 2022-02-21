
using System;
using System.Reactive;

using NetSparkleUpdater;
using NetSparkleUpdater.Events;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class UpdateDownloadingViewModel : ViewModelBase
{
    private double _downloadProgress;

    public double DownloadProgress
    {
        get => _downloadProgress;
        set => this.RaiseAndSetIfChanged(ref _downloadProgress, value);
    }

    public ReactiveCommand<bool, bool> CancelDownloadCommand { get; }

    public UpdateDownloadingViewModel() => CancelDownloadCommand = ReactiveCommand.Create<bool, bool>(v => v);

    internal void ChangeProgress(object sender, AppCastItem item, ItemDownloadProgressEventArgs args)
    {
        DownloadProgress = args.ProgressPercentage;
    }

    internal void UpdateFinished(AppCastItem item, string path) => CancelDownloadCommand?.Execute(false);
}
