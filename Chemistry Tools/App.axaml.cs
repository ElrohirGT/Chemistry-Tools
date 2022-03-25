using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Chemistry_Tools.Views;

namespace Chemistry_Tools;
public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
