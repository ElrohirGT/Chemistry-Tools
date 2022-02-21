using System;
using Avalonia.Markup.Xaml;

using Chemistry_Tools.ViewModels;
using Avalonia.ReactiveUI;
using System.Threading.Tasks;
using ReactiveUI;
using NetSparkleUpdater;
using Chemistry_Tools.Views.PopUps;
using Avalonia;

namespace Chemistry_Tools.Views;
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    readonly MainWindowViewModel _viewModel;
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        DataContext = _viewModel = new MainWindowViewModel();
        _viewModel.ShouldUpdateInteraction.RegisterHandler(DoShowDialogAsync);
        _viewModel.ShouldCancelDownload.RegisterHandler(CancelDialogAsync);
        _viewModel.Close += Close;
    }

    private async Task CancelDialogAsync(InteractionContext<SparkleUpdater, bool> interaction)
    {
        UpdateDownloadingWindow popUp = new();
        interaction.Input.DownloadMadeProgress -= popUp.ViewModel.ChangeProgress;
        interaction.Input.DownloadMadeProgress += popUp.ViewModel.ChangeProgress;

        interaction.Input.DownloadFinished += popUp.ViewModel.UpdateFinished;

        bool shouldCancel = await popUp.ShowDialog<bool>(this);
        interaction.SetOutput(shouldCancel);
    }

    protected override async void OnOpened(EventArgs e) => await _viewModel.OnOpened(e);
    public async Task DoShowDialogAsync(InteractionContext<AppCastItem, bool> interaction)
    {
        UpdatePopUpWindow popUp = new();
        if (popUp.UpdateInfoStackPanel is not null)
            popUp.UpdateInfoStackPanel.DataContext = interaction.Input;
        //TODO: Add a logging error here!

        bool result = await popUp.ShowDialog<bool>(this);
        interaction.SetOutput(result);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
