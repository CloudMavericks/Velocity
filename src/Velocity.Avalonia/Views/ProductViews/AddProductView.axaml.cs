using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Products;

namespace Velocity.Avalonia.Views.ProductViews;

public partial class AddProductView : ViewBase<AddProductViewModel>
{
    public AddProductView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<AddProductViewModel>();
        InitializeComponent();
    }
}