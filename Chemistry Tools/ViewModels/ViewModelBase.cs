using System;
using System.Threading.Tasks;

using Avalonia;
using FluentAvalonia.Styling;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class ViewModelBase : ReactiveObject
{
    public ViewModelBase()
    {
        var themeManager = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
        themeManager.RequestedThemeChanged += OnThemeChanged;
    }

    private void OnThemeChanged(FluentAvaloniaTheme sender, RequestedThemeChangedEventArgs args)
    {

    }

    public virtual Task OnOpened(EventArgs e) => Task.CompletedTask;
}
