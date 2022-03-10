namespace Chemistry_Tools.Core.Updaters;

public class UpdateItem
{
    public string DownloadPath { get; set; }
    public string UpdateVersion { get; set; }
    public string Title { get; set; }
    public long UpdateSize { get; set; }
    public string InstalledVersion { get; set; }
    public string ReleaseNotesLink { get; set; }
    public UpdateStatus Status { get; set; }
}