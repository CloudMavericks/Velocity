using CommunityToolkit.Mvvm.ComponentModel;
using Velocity.Avalonia.Services;
using Velocity.Avalonia.ViewModels.Customers;

namespace Velocity.Avalonia.ViewModels.Products;

public partial class ProductsViewModel : ViewModelBase, IDisposable
{
    private readonly NavigationService _navigationService;
    private bool _disposed;

    public ProductsViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigationService.NavigationChanged += OnNavigationChanged;
        _navigationService.NavigateTo<ProductsTableViewModel>();
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