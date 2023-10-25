using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia.Enums;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Services;
using Velocity.Shared.Responses.Products;
using Velocity.Shared.Wrapper;

namespace Velocity.Avalonia.ViewModels.Products;

public partial class ProductsTableViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public ProductsTableViewModel(HttpClient httpClient, NavigationService navigationService, LocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    public ObservableCollection<ProductResponse> Products { get; } = new();
    
    [ObservableProperty]
    private int _currentPage = 1;
    
    [ObservableProperty]
    private int _pageSize = 10;
    
    public ObservableCollection<int> Pages { get; private set; } = new() {1};
    
    public ObservableCollection<int> PageSizes { get; } = new() {10, 20, 50, 100};
    
    [RelayCommand]
    private void AddProduct()
    {
        _navigationService.NavigateTo<AddProductViewModel>();
    }
    
    [RelayCommand]
    private Task RefreshAsync()
    {
        return ReloadProducts();
    }

    public async Task ReloadProducts()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<PaginatedResult<ProductResponse>>(
                $"products?pageNumber={CurrentPage}&pageSize={PageSize}");
            if (response.Succeeded)
            {
                Products.Clear();
                foreach (var customer in response.Data)
                {
                    Products.Add(customer);
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
        await _navigationService.NavigateTo<EditProductViewModel>(id);
    }
    
    [RelayCommand]
    private async Task DeleteAsync(Guid id)
    {
        var result = await MessageBox.ShowDialogAsync("Delete", "Are you sure you want to delete this product?", ButtonEnum.YesNo);
        if (result == ButtonResult.Yes)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = "Unable to delete product : " + response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "Product already in use",
                    HttpStatusCode.NotFound => "Product not found",
                    _ => "Unknown error"
                };
                await MessageBox.ShowDialogAsync("Error", errorMessage);
            }
            else
            {
                await MessageBox.ShowDialogAsync("Delete", "Product deleted successfully");
                await ReloadProducts();
            }
        }
    }
}