using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia.Enums;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Services;
using Velocity.Shared.Responses.Suppliers;
using Velocity.Shared.Wrapper;

namespace Velocity.Avalonia.ViewModels.Suppliers;

public partial class SuppliersTableViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public SuppliersTableViewModel(HttpClient httpClient, NavigationService navigationService, LocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    public ObservableCollection<SupplierResponse> Suppliers { get; } = new();
    
    [ObservableProperty]
    private int _currentPage = 1;
    
    [ObservableProperty]
    private int _pageSize = 10;
    
    public ObservableCollection<int> Pages { get; private set; } = new() {1};
    
    public ObservableCollection<int> PageSizes { get; } = new() {10, 20, 50, 100};
    
    [RelayCommand]
    private void AddSupplier()
    {
        _navigationService.NavigateTo<AddSupplierViewModel>();
    }
    
    [RelayCommand]
    private Task RefreshAsync()
    {
        return ReloadSuppliers();
    }
    
    public async Task ReloadSuppliers()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedResult<SupplierResponse>>(
                $"suppliers?pageNumber={CurrentPage}&pageSize={PageSize}");
            if (response.Succeeded)
            {
                Suppliers.Clear();
                foreach (var supplier in response.Data)
                {
                    Suppliers.Add(supplier);
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
            await MessageBox.ShowDialogAsync("Error", e.Message);;
        }
    }
    
    [RelayCommand]
    private async Task EditAsync(Guid id)
    {
        await _navigationService.NavigateTo<EditSupplierViewModel>(id);
    }
    
    [RelayCommand]
    private async Task DeleteAsync(Guid id)
    {
        var confirm = await MessageBox.ShowDialogAsync("Confirm", "Are you sure you want to delete this supplier?", ButtonEnum.YesNo);
        if (confirm == ButtonResult.Yes)
        {
            var response = await _httpClient.DeleteAsync($"suppliers/{id}");
            if (response.IsSuccessStatusCode)
            {
                await ReloadSuppliers();
                await MessageBox.ShowDialogAsync("Success", "Supplier deleted successfully");
            }
            else
            {
                var errorMessage = "Unable to delete supplier : " + response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "Supplier has purchase invoices",
                    HttpStatusCode.NotFound => "Supplier not found",
                    _ => "Unknown error"
                };
                await MessageBox.ShowDialogAsync("Error", errorMessage);
            }
        }
    }
}