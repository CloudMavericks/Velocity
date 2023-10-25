﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Services;
using Velocity.Shared.Requests.Products;
using Velocity.Shared.Responses.Suppliers;
using Velocity.Shared.Wrapper;

namespace Velocity.Avalonia.ViewModels.Products;

public partial class AddProductViewModel : ViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public AddProductViewModel(HttpClient httpClient, LocalStorageService localStorageService, NavigationService navigationService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [ObservableProperty]
    private CreateProductRequest _product = new();
    
    [ObservableProperty]
    private string _supplier;
    
    private ICollection<SupplierResponse> _suppliers = new List<SupplierResponse>();
    
    public async Task<IEnumerable<object>> GetSuppliers(string text, CancellationToken cancellationToken)
    {
        await MessageBox.ShowDialogAsync("Error", "Not Working");
        var response = await _httpClient.GetFromJsonAsync<PaginatedResult<SupplierResponse>>($"suppliers?searchString={text}&pageSize=5", cancellationToken);
        _suppliers = response.Data;
        return _suppliers.Select(x => x.Name);
    }

    [RelayCommand]
    private async Task SaveProductAsync()
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("products", Product);
            response.EnsureSuccessStatusCode();
            await MessageBox.ShowDialogAsync("Success", "Product added successfully");
            _navigationService.NavigateTo<ProductsTableViewModel>();
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message);
        }
    }
    
    [RelayCommand]
    private void GoBack()
    {
        _navigationService.NavigateTo<ProductsTableViewModel>();
    }
}