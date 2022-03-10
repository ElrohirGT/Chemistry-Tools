
using System.Diagnostics;
using System.IO.Compression;

using Chemistry_Tools.Core.Updaters;

namespace Chemistry_Tools.Infrastructure.UpdatesIntallers;
public class OSXUpdateInstaller : IUpdateInstaller
{
    public void Install(string filePath)
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
}
