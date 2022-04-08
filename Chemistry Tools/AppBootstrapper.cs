using System.Runtime.InteropServices;

using Chemistry_Tools.UserSettings;
using Chemistry_Tools.Core.Updaters;
using Chemistry_Tools.Infrastructure;
using Chemistry_Tools.Infrastructure.UpdatesIntallers;
using Chemistry_Tools.ViewModels;

using Splat;
using ReactiveUI;
using Chemistry_Tools.Views;
using Chemistry_Tools.Core.Services.PeriodicTableService;
using Chemistry_Tools.Infrastructure.Services;

namespace Chemistry_Tools;

internal static class AppBootstrapper
{
    internal static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        //Updaters
        services.Register(() =>
        {
            IUpdateInstaller? installer = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                installer = new WindowsUpdateInstaller();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                installer = new LinuxUpdateInstaller();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                installer = new OSXUpdateInstaller();
            return installer;
        });
        //TODO: Use the real updater
        //services.Register<IUpdater>(() => new Updater(resolver.GetService<IUpdateInstaller>()));
        services.Register<IUpdater>(() => new TestUpdater(resolver.GetService<IUpdateInstaller>()));

        services.RegisterLazySingleton<IPeriodicTableService>(() => new PeriodicTable());

        //Settings
        services.RegisterLazySingleton<IUserSettings>(() => new FluentUserSettings().Parse());

        //ViewModels
        services.Register(() => new ErrorPopUpViewModel(resolver.GetService<IUserSettings>()));
        services.Register(() => new UpdateDownloadingViewModel(resolver.GetService<IUserSettings>()));
        services.Register(() => new UpdatePopUpViewModel(resolver.GetService<IUserSettings>()));
        services.Register(() => new MainWindowViewModel(resolver.GetService<IUpdater>(), resolver.GetService<IUserSettings>()));

        //Routes
        services.RegisterViewsForViewModels(typeof(AppBootstrapper).Assembly);
        //services.Register<IViewFor<ConfigurationViewModel>>(() => new ConfigurationView());
        //services.Register<IViewFor<HomeViewModel>>(() => new HomeView());
        //services.Register<IViewFor<MolCalculatorViewModel>>(() => new MolCalculatorView());
    }
}