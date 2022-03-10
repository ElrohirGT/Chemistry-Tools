using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chemistry_Tools.Core.Updaters;
public interface IUpdater
{
    event Action<UpdateItem, Exception> DownloadHadError;
    event Action<float> DownloadMadeProgress;
    event Action CloseApplication;
    event Action<string> DownloadStarted;
    event Action<string> DownloadFinished;

    Task<UpdateItem?> CheckForUpdatesQuietly();
    Task InitAndBeginDownload();
    void CancelFileDownload();
    void InstallUpdate(string path);
}
