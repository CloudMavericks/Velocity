using Velocity.Avalonia.Interfaces;
using Velocity.Avalonia.ViewModels;

namespace Velocity.Avalonia.Services;

public class NavigationService
{
    private ViewModelBase _currentViewModel;
    
    public event Action NavigationChanged;
    
    private void OnNavigationChanged() => NavigationChanged?.Invoke();
    
    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        CurrentViewModel = App.ServiceProvider.GetService(typeof(TViewModel)) as ViewModelBase;
    }

    public async Task NavigateTo<TViewModel>(object parameters) where TViewModel : ViewModelBase, IViewModelInitialize
    {
        CurrentViewModel = App.ServiceProvider.GetService(typeof(TViewModel)) as ViewModelBase;
        if (CurrentViewModel is IViewModelInitialize viewModelInitialize)
        {
            await viewModelInitialize.Initialize(parameters);
        }
    }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            _currentViewModel = value;
            OnNavigationChanged();
        }
    }
}