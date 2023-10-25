using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Suppliers;

namespace Velocity.Avalonia.Views.SupplierViews;

public partial class AddSupplierView : ViewBase<AddSupplierViewModel>
{
    public AddSupplierView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<AddSupplierViewModel>();
        InitializeComponent();
    }
}