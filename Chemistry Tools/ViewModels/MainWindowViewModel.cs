using System;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Chemistry_Tools.Views.PopUps;

using NetSparkleUpdater;
using NetSparkleUpdater.SignatureVerifiers;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private readonly SparkleUpdater _sparkle;
    public string Greeting => "Welcome to Avalonia!";

    public Interaction<AppCastItem, bool> ShouldUpdateInteraction { get; } = new Interaction<AppCastItem, bool>();
    public Interaction<SparkleUpdater, bool> ShouldCancelDownload { get; } = new Interaction<SparkleUpdater, bool>();
    public event Action Close;

    public MainWindowViewModel()
    {
        _sparkle = ConfigureUpdater();
        _sparkle.StartLoop(true, true);
    }

    public override async Task OnOpened(EventArgs e)
    {
        await base.OnOpened(e);
        var update = await _sparkle.CheckForUpdatesQuietly();
        await InstallNewUpdateAsync(update);
    }

    private async Task InstallNewUpdateAsync(UpdateInfo update)
    {
        if (update.Status != NetSparkleUpdater.Enums.UpdateStatus.UpdateAvailable)
            return;

        var appCastItem = update.Updates[0];
        var shouldUpdate = await ShouldUpdateInteraction.Handle(appCastItem);
        if (!shouldUpdate)
            return;

        _sparkle.CloseApplication -= OnCloseApplication;
        _sparkle.CloseApplication += OnCloseApplication;

        _sparkle.DownloadStarted -= DownloadStarted;
        _sparkle.DownloadStarted += DownloadStarted;
        
        await _sparkle.InitAndBeginDownload(appCastItem);
        _sparkle.InstallUpdate(appCastItem);

        //TODO: Add a way to skip a version here.
    }

    private async void DownloadStarted(AppCastItem item, string path)
    {
        _sparkle.DownloadStarted -= DownloadStarted;
        var shouldCancel = await ShouldCancelDownload.Handle(_sparkle);
        if (shouldCancel)
            _sparkle.CancelFileDownload();
    }

    private void OnCloseApplication() => Close?.Invoke();

    private SparkleUpdater ConfigureUpdater()
    {
        string osPlatform = string.Empty;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            osPlatform = "windows";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            osPlatform = "linux";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            osPlatform = "macos";
        string appCastUrl = $"https://chemistry-tools.netlify.app/{osPlatform}/appcast.xml";

        string key = "+tmxn2gyjXh4VK5AykT5HT/E9qEM34/+GTaspXU1dWA=";
        SparkleUpdater sparkle = new(appCastUrl,
                    new Ed25519Checker(NetSparkleUpdater.Enums.SecurityMode.UseIfPossible, key))
        {
            ShowsUIOnMainThread = true,
            UseNotificationToast = false
        };
        return sparkle;
    }
}
