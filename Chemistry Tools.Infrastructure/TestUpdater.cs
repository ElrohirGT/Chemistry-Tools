
using System.Runtime.InteropServices;

using Chemistry_Tools.Core.Updaters;

namespace Chemistry_Tools.Infrastructure;
public class TestUpdater : IUpdater
{
    private readonly string DOWNLOAD_FILE = @"C:\windows\system32\notepad.exe";
    readonly CancellationTokenSource _cts = new();
    private readonly IUpdateInstaller _installer;

    public TestUpdater(IUpdateInstaller installer)
    {
        _installer = installer;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            DOWNLOAD_FILE = @"C:\windows\system32\notepad.exe";
        else
            DOWNLOAD_FILE = "/";
    }

    public event Action? CloseApplication;
    public event Action<string>? DownloadStarted;
    public event Action<string>? DownloadFinished;
    public event Action<UpdateItem, Exception>? DownloadHadError;
    public event Action<float>? DownloadMadeProgress;

    public void CancelFileDownload() => _cts.Cancel();
    public async Task<UpdateItem?> CheckForUpdatesQuietly()
    {
        await Task.Delay(2000);
        return new UpdateItem
        {
            DownloadPath = DOWNLOAD_FILE,
            Title = "Chemistry Tools",
            Status = UpdateStatus.UpdateAvailable,
            UpdateVersion = "999.999.999",
            InstalledVersion = "0.0.0",
            UpdateSize = 125_000,
            ReleaseNotesLink = "https://url.com/RELEASE.md"
        };
    }

    public async Task InitAndBeginDownload()
    {
        DownloadStarted?.Invoke(DOWNLOAD_FILE);
        await Task.Run(async () =>
        {
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(50);
                DownloadMadeProgress?.Invoke(i);
            }
        }, _cts.Token);
        DownloadFinished?.Invoke(DOWNLOAD_FILE);
    }

    //This mock is used to test the updating process, but it doesn't really install a new update.
    public void InstallUpdate(string path)
    {
        CloseApplication?.Invoke();
        _installer.Install(path);
    }
}
