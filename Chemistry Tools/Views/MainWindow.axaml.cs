using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

using NetSparkleUpdater;
using NetSparkleUpdater.SignatureVerifiers;

namespace Chemistry_Tools.Views;
public partial class MainWindow : Window
{
    private readonly SparkleUpdater _sparkle;

    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        string key = "+tmxn2gyjXh4VK5AykT5HT/E9qEM34/+GTaspXU1dWA=";
        _sparkle = new SparkleUpdater(
            "https://chemistry-tools.netlify.app/windows/appcast.xml",
            new Ed25519Checker(NetSparkleUpdater.Enums.SecurityMode.UseIfPossible, key))
        {
            UIFactory = new NetSparkleUpdater.UI.Avalonia.UIFactory(Icon),
            ShowsUIOnMainThread = true,
            UseNotificationToast = false
        };

        _sparkle.StartLoop(true, true);
    }

    protected override async void OnOpened(EventArgs e)
    {
        try
        {
            var result = await _sparkle.CheckForUpdatesAtUserRequest();
        }
        catch (Exception err)
        {

            throw;
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
