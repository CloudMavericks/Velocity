using Avalonia.Controls;
using Velocity.Avalonia.ViewModels;

namespace Velocity.Avalonia.Views;

public abstract class ViewBase<T> : UserControl where T : ViewModelBase
{
    protected T ViewModel
    {
        get => DataContext as T;
        set => DataContext = value;
    }
}