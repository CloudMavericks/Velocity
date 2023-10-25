using Microsoft.Extensions.DependencyInjection;
using Velocity.Avalonia.ViewModels.Suppliers;

namespace Velocity.Avalonia.Views.SupplierViews;

public partial class EditSupplierView : ViewBase<EditSupplierViewModel>
{
    public EditSupplierView()
    {
        DataContext = App.ServiceProvider.GetRequiredService<EditSupplierViewModel>();
        InitializeComponent();
    }
}