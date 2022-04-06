using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Chemistry_Tools.UserSettings;
using Chemistry_Tools.Core.Updaters;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class MainWindowViewModel : ViewModelBase, IScreen
{
    private readonly IUpdater _updater;

    public Interaction<UpdateItem, bool> ShouldUpdateInteraction { get; } = new Interaction<UpdateItem, bool>();
    public Interaction<IUpdater, bool> ShouldCancelDownload { get; } = new Interaction<IUpdater, bool>();
    public Interaction<Exception, Unit> ShowError { get; } = new Interaction<Exception, Unit>();
    public ReactiveCommand<Unit, IRoutableViewModel> GoToConfigurationCommand { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> GoHome { get; }

    public RoutingState Router { get; } = new RoutingState();

    public event Action? Close;

    public MainWindowViewModel(IUpdater updater, IUserSettings appSettings) : base(appSettings)
    {
        _updater = updater;
        GoHome = ReactiveCommand.CreateFromObservable(() => Router.NavigateAndReset.Execute(new HomeViewModel(appSettings, this)));
        GoToConfigurationCommand = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ConfigurationViewModel(appSettings, this)));
    }

    public override async Task OnOpened(EventArgs e)
    {
        await base.OnOpened(e);
        UpdateItem? update = await _updater.CheckForUpdatesQuietly();
        if (update is not null)
            await DownloadNewUpdateAsync(update);
    }

    private async Task DownloadNewUpdateAsync(UpdateItem update)
    {
        var shouldUpdate = await ShouldUpdateInteraction.Handle(update);
        //TODO: Add a way to skip a version here.
        if (!shouldUpdate)
            return;

        _updater.CloseApplication -= OnCloseApplication;
        _updater.CloseApplication += OnCloseApplication;

        _updater.DownloadStarted -= DownloadStarted;
        _updater.DownloadStarted += DownloadStarted;

        _updater.DownloadFinished -= DownloadFinished;
        _updater.DownloadFinished += DownloadFinished;

        await _updater.InitAndBeginDownload();
    }

    private async void DownloadFinished(string path)
    {
        await Task.Delay(200).ConfigureAwait(true);

        try
        {
            _updater.InstallUpdate(path);
        }
        catch (Exception e)
        {
            await ShowError.Handle(e);
            OnCloseApplication();
        }
    }

    private async void DownloadStarted(string path)
    {
        _updater.DownloadStarted -= DownloadStarted;
        var shouldCancel = await ShouldCancelDownload.Handle(_updater);
        if (shouldCancel)
            _updater.CancelFileDownload();
    }

    public override async Task OnClosed(EventArgs e)
    {
        await base.OnClosed(e);
        await UserSettings.Save();
        UserSettings.Dispose();
    }

    private void OnCloseApplication() => Close?.Invoke();
}
