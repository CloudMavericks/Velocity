using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Products;

namespace Velocity.Avalonia.Views.ProductViews;

public partial class ProductsView : ViewBase<ProductsViewModel>
{
    public ProductsView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<ProductsViewModel>();
        InitializeComponent();
    }
}