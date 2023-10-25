using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Products;

namespace Velocity.Avalonia.Views.ProductViews;

public partial class ProductsTableView : ViewBase<ProductsTableViewModel>
{
    public ProductsTableView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<ProductsTableViewModel>();
        InitializeComponent();
    }

    private async void Control_OnLoaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.ReloadProducts();
    }
}