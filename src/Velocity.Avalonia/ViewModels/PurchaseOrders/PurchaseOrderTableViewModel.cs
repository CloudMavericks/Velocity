using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia.Enums;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Services;
using Velocity.Shared.Responses.PurchaseOrders;
using Velocity.Shared.Wrapper;

namespace Velocity.Avalonia.ViewModels.PurchaseOrders;

public partial class PurchaseOrderTableViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public PurchaseOrderTableViewModel(HttpClient httpClient, NavigationService navigationService, LocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    public ObservableCollection<PurchaseOrderResponse> PurchaseOrders { get; } = new();
    
    [ObservableProperty]
    private int _currentPage = 1;
    
    [ObservableProperty]
    private int _pageSize = 10;
    
    public ObservableCollection<int> Pages { get; private set; } = new() {1};
    
    public ObservableCollection<int> PageSizes { get; } = new() {10, 20, 50, 100};
    
    [RelayCommand]
    private void AddPurchaseOrder()
    {
        _navigationService.NavigateTo<AddPurchaseOrderViewModel>();
    }
    
    [RelayCommand]
    private Task RefreshAsync()
    {
        return ReloadPurchaseOrders();
    }

    public async Task ReloadPurchaseOrders()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedResult<PurchaseOrderResponse>>(
                $"purchase-orders?pageNumber={CurrentPage}&pageSize={PageSize}");
            if (response.Succeeded)
            {
                PurchaseOrders.Clear();
                foreach (var customer in response.Data)
                {
                    PurchaseOrders.Add(customer);
                }
                Pages = new ObservableCollection<int>(Enumerable.Range(1, (int)response.TotalPages));
            }
            else
            {
                await MessageBox.ShowDialogAsync("Error", response.Messages.First() ?? "Unknown error");
            }
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message);
        }
    }
    
    [RelayCommand]
    private async Task EditAsync(Guid id)
    {
        await _navigationService.NavigateTo<EditPurchaseOrderViewModel>(id);
    }
    
    [RelayCommand]
    private async Task DeleteAsync(Guid id)
    {
        var result = await MessageBox.ShowDialogAsync("Delete", "Are you sure you want to delete this purchase order?", ButtonEnum.YesNo);
        if (result == ButtonResult.Yes)
        {
            var response = await _httpClient.DeleteAsync($"purchase-orders/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = "Unable to delete purchase order : " + response.StatusCode switch
                {
                    HttpStatusCode.NotFound => "Purchase Order not found",
                    _ => "Unknown error"
                };
                await MessageBox.ShowDialogAsync("Error", errorMessage);
            }
            else
            {
                await MessageBox.ShowDialogAsync("Delete", "Purchase Order deleted successfully");
                await ReloadPurchaseOrders();
            }
        }
    }
}