using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reactive;
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
    public Interaction<Exception, Unit> ShowError { get; } = new Interaction<Exception, Unit>();

    public event Action? Close;

    public MainWindowViewModel()
    {
        _sparkle = ConfigureUpdater();
        _sparkle.StartLoop(true, true);
    }

    public override async Task OnOpened(EventArgs e)
    {
        await base.OnOpened(e);
        var update = await _sparkle.CheckForUpdatesQuietly();
        await DownloadNewUpdateAsync(update);
    }

    private async Task DownloadNewUpdateAsync(UpdateInfo update)
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

        _sparkle.DownloadFinished -= DownloadFinished;
        _sparkle.DownloadFinished += DownloadFinished;

        await _sparkle.InitAndBeginDownload(appCastItem);

        //TODO: Add a way to skip a version here.
    }

    private async void DownloadFinished(AppCastItem item, string path)
    {
        await Task.Delay(200);
        //_sparkle.InstallUpdate(item, path);

        try
        {
            //TODO: Windows and macOS should follow this route, but macOS is currently a .zip file, that's not an installer.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _sparkle.InstallUpdate(item);
                return;
            }

            string fileName = Path.GetFileName(path);
            string outputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "chTools You can delete this");
            string destFileName = Path.Combine(outputDir, fileName);
            Directory.CreateDirectory(outputDir);
            File.Move(path, destFileName);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                InstallApplicationOnMacOS(destFileName);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                InstallApplicationOnLinux(outputDir, destFileName);

            OnCloseApplication();
        }
        catch (Exception e)
        {
            await ShowError.Handle(e);
            OnCloseApplication();
        }
    }

    private async void InstallApplicationOnLinux(string pathToOpen, string destFileName)
    {
        //TODO: Check if this opens a terminal in linux or not
        ProcessStartInfo p = new()
        {
            FileName = "cd",
            Arguments = pathToOpen,
            CreateNoWindow = false
        };
        Process.Start(p);
    }

    private void InstallApplicationOnMacOS(string filePath)
    {
        var parentDir = Path.GetDirectoryName(filePath);
        ZipFile.ExtractToDirectory(filePath, parentDir, true);
        File.Delete(filePath);
        ProcessStartInfo p = new()
        {
            FileName = "open",
            Arguments = $"-a Finder {parentDir}"
        };
        Process.Start(p);

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
