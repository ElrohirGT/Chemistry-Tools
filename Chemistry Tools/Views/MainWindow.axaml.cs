using System;
using Avalonia.Markup.Xaml;

using Chemistry_Tools.ViewModels;
using Avalonia.ReactiveUI;
using System.Threading.Tasks;
using ReactiveUI;
using NetSparkleUpdater;
using Chemistry_Tools.Views.PopUps;
using Avalonia;
using System.Reactive;
using FluentAvalonia.UI.Controls;

namespace Chemistry_Tools.Views;
public partial class MainWindow : CoreWindow
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
        _viewModel.ShowError.RegisterHandler(ShowErrorDialogAsync);
        _viewModel.Close += Close;
        TitleBar.ExtendViewIntoTitleBar = true;
        //SetTitleBar(TitleBar);
    }

    private async Task ShowErrorDialogAsync(InteractionContext<Exception, Unit> interaction)
    {
        ErrorPopUpWindow errorPoUp = new();
        errorPoUp.ViewModel.Error = interaction.Input;

        var result = await errorPoUp.ShowDialog<Unit>(this);
        interaction.SetOutput(result);
    }

    private async Task CancelDialogAsync(InteractionContext<SparkleUpdater, bool> interaction)
    {
        UpdateDownloadingWindow popUp = new();
        interaction.Input.DownloadMadeProgress -= popUp.ViewModel.ChangeProgress;
        interaction.Input.DownloadMadeProgress += popUp.ViewModel.ChangeProgress;

        interaction.Input.DownloadFinished -= popUp.ViewModel.UpdateFinished;
        interaction.Input.DownloadFinished += popUp.ViewModel.UpdateFinished;

        interaction.Input.DownloadHadError -= popUp.ViewModel.DownloadHadError;
        interaction.Input.DownloadHadError += popUp.ViewModel.DownloadHadError;

        bool shouldCancel = await popUp.ShowDialog<bool>(this);
        interaction.SetOutput(shouldCancel);
    }

    protected override async void OnOpened(EventArgs e) => await _viewModel.OnOpened(e);
    public async Task DoShowDialogAsync(InteractionContext<AppCastItem, bool> interaction)
    {
        UpdatePopUpWindow popUp = new();
        popUp.ViewModel.AppCastItem = interaction.Input;

        bool result = await popUp.ShowDialog<bool>(this);
        interaction.SetOutput(result);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
