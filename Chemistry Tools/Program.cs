using System;

using Avalonia;
using Avalonia.ReactiveUI;

using Splat;

namespace Chemistry_Tools;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        AppBootstrapper.Register(Locator.CurrentMutable, Locator.Current);
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var loggingLevel = Avalonia.Logging.LogEventLevel.Warning;
#if DEBUG
        loggingLevel = Avalonia.Logging.LogEventLevel.Debug;
#endif
        return AppBuilder.Configure<App>()
                   .UsePlatformDetect()
                   .LogToTrace(loggingLevel)
                   .UseReactiveUI();
    }
}
