using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Services;
using Velocity.Shared.Requests.PurchaseOrders;
using Velocity.Shared.Responses.Suppliers;
using Velocity.Shared.Wrapper;

namespace Velocity.Avalonia.ViewModels.PurchaseOrders;

public partial class AddPurchaseOrderViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public AddPurchaseOrderViewModel(HttpClient httpClient, NavigationService navigationService, LocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [ObservableProperty]
    private CreatePurchaseOrderRequest _purchaseOrder = new();
    
    [ObservableProperty]
    private SupplierResponse _selectedSupplier;

    private ObservableCollection<SupplierResponse> Suppliers { get; set; } = new();
    
    public AvaloniaList<CreatePurchaseOrderItemRequest> Items { get; set; } = new();
    
    public async Task<IEnumerable<object>> GetSuppliers(string text, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<PaginatedResult<SupplierResponse>>($"suppliers?pageNumber=1&pageSize=5&searchString={text}", cancellationToken);
        Suppliers.Clear();
        foreach (var supplier in response.Data)
        {
            Suppliers.Add(supplier);
        }
        return Suppliers;
    }
    
    [RelayCommand]
    private void AddItem()
    {
        Items.Add(new CreatePurchaseOrderItemRequest() {ProductId = Guid.Empty, Quantity = 1, UnitPrice = 0, DiscountAmount = 0, TaxPercentage = 0});
    }
    
    [RelayCommand(CanExecute = nameof(CanSavePurchaseOrderAsync))]
    private async Task SavePurchaseOrderAsync()
    {
        try
        {
            if(PurchaseOrder.Items.Any(x => x.ProductId == Guid.Empty))
                throw new Exception("Please don't leave any empty rows");
            if(PurchaseOrder.Items.Any(x => x.Quantity == 0))
                throw new Exception("Quantity cannot be zero for any item");
            PurchaseOrder.SupplierId = SelectedSupplier.Id;
            var response = await _httpClient.PostAsJsonAsync("purchase-orders", PurchaseOrder);
            response.EnsureSuccessStatusCode();
            await MessageBox.ShowDialogAsync("Success", "Purchase order added successfully");
            _navigationService.NavigateTo<PurchaseOrderTableViewModel>();
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message);
        }
    }
    
    private bool CanSavePurchaseOrderAsync()
    {
        return PurchaseOrder.SupplierId != Guid.Empty 
               && PurchaseOrder.Items.Any() 
               && PurchaseOrder.Items.All(x => 
                   x.ProductId != Guid.Empty 
                   && x.Quantity > 0 
                   && x.Total > 0);
    }
    
    [RelayCommand]
    private void GoBack()
    {
        _navigationService.NavigateTo<PurchaseOrderTableViewModel>();
    }
}