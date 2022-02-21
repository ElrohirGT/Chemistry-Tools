using System;
using System.Threading.Tasks;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class ViewModelBase : ReactiveObject
{
    public virtual Task OnOpened(EventArgs e) => Task.CompletedTask;
}
