using CommunityToolkit.Mvvm.ComponentModel;
using Velocity.Avalonia.Services;

namespace Velocity.Avalonia.ViewModels.Customers;

public partial class CustomerViewModel : ViewModelBase, IDisposable
{
    private readonly NavigationService _navigationService;
    private bool _disposed;

    public CustomerViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigationService.NavigationChanged += OnNavigationChanged;
        _navigationService.NavigateTo<CustomersTableViewModel>();
    }

    private void OnNavigationChanged()
    {
        CurrentViewModel = _navigationService.CurrentViewModel;
    }

    [ObservableProperty]
    private ViewModelBase _currentViewModel;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _navigationService.NavigationChanged -= OnNavigationChanged;
        }
        _disposed = true;
    }
}