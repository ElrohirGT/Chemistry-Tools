
using System.Diagnostics;

using Chemistry_Tools.Core.Updaters;

namespace Chemistry_Tools.Infrastructure.UpdatesIntallers;
public class LinuxUpdateInstaller : IUpdateInstaller
{
    public void Install(string filePath)
    {
        //TODO: Check if this opens a terminal in linux or not
        ProcessStartInfo p = new()
        {
            FileName = "cd",
            Arguments = filePath,
            CreateNoWindow = false
        };
        Process.Start(p);
    }
}
