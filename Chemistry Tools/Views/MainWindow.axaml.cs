using Avalonia.Controls;
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
            "https://mega.nz/file/9EFWnLaZ#UBeO6LU0_8oolhBKOvNZTRqrVRGth7E4Hbrbb0DbmAY",
            new Ed25519Checker(NetSparkleUpdater.Enums.SecurityMode.Strict, key))
        {
            UIFactory = new NetSparkleUpdater.UI.Avalonia.UIFactory(Icon),
            ShowsUIOnMainThread = true,
        };

        _sparkle.StartLoop(true, true);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
