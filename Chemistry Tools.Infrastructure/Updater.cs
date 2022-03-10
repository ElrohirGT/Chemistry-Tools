using System.Runtime.InteropServices;

using Chemistry_Tools.Core.Updaters;

using NetSparkleUpdater;
using NetSparkleUpdater.SignatureVerifiers;

namespace Chemistry_Tools.Infrastructure;
public class Updater : IUpdater
{
    const string KEY = "+tmxn2gyjXh4VK5AykT5HT/E9qEM34/+GTaspXU1dWA=";
    readonly SparkleUpdater _updater;
    private AppCastItem _mostRecentUpdate;
    private readonly IUpdateInstaller _installer;

    public Updater(IUpdateInstaller installer)
    {
        string appCastUrl = GetAppCastUrl();
        _updater = new SparkleUpdater(appCastUrl, new Ed25519Checker(NetSparkleUpdater.Enums.SecurityMode.UseIfPossible, KEY));
        _updater.StartLoop(true, true);
        _installer = installer;

        _updater.CloseApplication += () => CloseApplication?.Invoke();
        _updater.DownloadStarted += (a, path) => DownloadStarted?.Invoke(path);
        _updater.DownloadFinished += (a, path) => DownloadFinished?.Invoke(path);
        _updater.DownloadHadError += (appcastItem, b, exception) =>
        {
            var updateItem = ConvertToUpdateItem(appcastItem);
            DownloadHadError?.Invoke(updateItem, exception);
        };
        _updater.DownloadMadeProgress += (a,b,eventArgs)=> DownloadMadeProgress?.Invoke(eventArgs.ProgressPercentage);
    }

    private static string GetAppCastUrl()
    {
        string osPlatform = string.Empty;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            osPlatform = "windows";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            osPlatform = "linux";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            osPlatform = "macos";
        string appCastUrl = $"https://chemistry-tools.netlify.app/{osPlatform}/appcast.xml";
        return appCastUrl;
    }

    public event Action CloseApplication;
    public event Action<string> DownloadStarted;
    public event Action<string> DownloadFinished;
    public event Action<UpdateItem, Exception> DownloadHadError;
    public event Action<float> DownloadMadeProgress;

    public void CancelFileDownload() => _updater.CancelFileDownload();

    public async Task<UpdateItem?> CheckForUpdatesQuietly()
    {
        var update = await _updater.CheckForUpdatesQuietly();
        if (update.Status != NetSparkleUpdater.Enums.UpdateStatus.UpdateAvailable)
            return null;
        
        _mostRecentUpdate = update.Updates[0];
        var updateItem = ConvertToUpdateItem(_mostRecentUpdate);

        updateItem.Status = ConvertStatus(update.Status);
        return updateItem;
    }

    private static UpdateItem ConvertToUpdateItem(AppCastItem appCastItem)
    {
        return new UpdateItem
        {
            Title = appCastItem.Title,
            InstalledVersion = appCastItem.AppVersionInstalled,
            UpdateVersion = appCastItem.Version,
            UpdateSize = appCastItem.UpdateSize,
            ReleaseNotesLink = appCastItem.ReleaseNotesLink
        };
    }

    private static UpdateStatus ConvertStatus(NetSparkleUpdater.Enums.UpdateStatus status) => (UpdateStatus)status;
    public Task InitAndBeginDownload() => _updater.InitAndBeginDownload(_mostRecentUpdate);

    public void InstallUpdate(string path) => _installer.Install(path);
}
