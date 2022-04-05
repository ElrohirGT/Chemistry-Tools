using System;
using System.IO;

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
        try
        {
            var appDomain = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(appDomain);
            AppBootstrapper.Register(Locator.CurrentMutable, Locator.Current);
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            string fileName = "chToolsLogFile.txt";
            var currentWorkingDir = Directory.GetCurrentDirectory();
            var exePath = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            var appDomain = AppDomain.CurrentDomain.BaseDirectory;

            using var stream = File.CreateText(Path.Combine(appDomain, fileName));
            stream.WriteLine(new string('=', 15));
            stream.WriteLine($"Current Working Directory: {currentWorkingDir}");
            stream.WriteLine($"EXE Path: {exePath}");
            stream.WriteLine($"App Domain: {appDomain}");
            stream.WriteLine(new string('=', 15));
            stream.WriteLine();

            LogExceptionToFile(e, stream);

            throw;
        }
    }

    private static void LogExceptionToFile(Exception? e, StreamWriter stream, int padding = 0)
    {
        if (e is null)
            return;
        if (e is AggregateException aggregate)
            foreach (var exception in aggregate.InnerExceptions)
                LogExceptionToFile(exception, stream);

        while (e is not null)
        {
            stream.WriteLine($"{new string('\t', padding)}{e}");
            LogExceptionToFile(e.InnerException, stream, padding+1);
            e = e.InnerException;
        }
        stream.Flush();
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
