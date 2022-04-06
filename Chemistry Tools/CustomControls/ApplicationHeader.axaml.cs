using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Chemistry_Tools.CustomControls;
public partial class ApplicationHeader : UserControl
{
    public ApplicationHeader()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
