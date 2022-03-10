
using System.Diagnostics;

using Chemistry_Tools.Core.Updaters;

namespace Chemistry_Tools.Infrastructure.UpdatesIntallers;
public class WindowsUpdateInstaller : IUpdateInstaller
{
    //TODO: Implement
    public void Install(string path)
    {
        ProcessStartInfo pInfo = new()
        {
            FileName = path
        };
        Process.Start(pInfo);
    }
}
