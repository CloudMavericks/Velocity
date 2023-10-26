using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Products;

namespace Velocity.Avalonia.Views.ProductViews;

public partial class EditProductView : ViewBase<EditProductViewModel>
{
    public EditProductView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<EditProductViewModel>();
        InitializeComponent();
    }
    
    private void Control_OnLoaded(object sender, RoutedEventArgs e)
    {
        var txtSupplier = this.Get<AutoCompleteBox>("TxtSupplier");
        txtSupplier.AsyncPopulator = ViewModel.GetSuppliers;
    }
}