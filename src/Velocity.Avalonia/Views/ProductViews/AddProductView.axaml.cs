using Avalonia.Controls;
using Avalonia.Interactivity;
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

    private void Control_OnLoaded(object sender, RoutedEventArgs e)
    {
        var txtSupplier = this.Get<AutoCompleteBox>("TxtSupplier");
        txtSupplier.AsyncPopulator = ViewModel.GetSuppliers;
    }
}