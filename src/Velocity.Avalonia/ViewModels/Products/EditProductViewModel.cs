using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velocity.Avalonia.Helpers;
using Velocity.Avalonia.Interfaces;
using Velocity.Avalonia.Services;
using Velocity.Shared.Requests.Products;
using Velocity.Shared.Responses.Products;
using Velocity.Shared.Responses.Suppliers;
using Velocity.Shared.Wrapper;

namespace Velocity.Avalonia.ViewModels.Products;

public partial class EditProductViewModel : ViewModelBase, IViewModelInitialize
{
    private readonly HttpClient _httpClient;
    private readonly NavigationService _navigationService;

    public EditProductViewModel(HttpClient httpClient, LocalStorageService localStorageService, NavigationService navigationService)
    {
        _httpClient = httpClient;
        _navigationService = navigationService;
        var token = localStorageService.GetItem<string>("token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    [ObservableProperty]
    private UpdateProductRequest _product = new();

    public async ValueTask Initialize(object parameter)
    {
        if (parameter is Guid id)
        {
            try
            {
                var customerResponse = await _httpClient.GetFromJsonAsync<ProductResponse>($"products/{id}");
                Product = new UpdateProductRequest()
                {
                    Id = customerResponse.Id,
                    Name = customerResponse.Name,
                    Description = customerResponse.Description,
                    AlertQuantity = customerResponse.AlertQuantity,
                    SupplierId = customerResponse.SupplierId,
                };
            }
            catch (Exception e)
            {
                if (e is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
                {
                    await MessageBox.ShowDialogAsync("Error", "Product not found");
                }
                else
                {
                    await MessageBox.ShowDialogAsync("Error", e.Message);
                }
            }
        }
        else
        {
            await MessageBox.ShowDialogAsync("Error", "Invalid parameter");
        }
    }
    
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
            var response = await _httpClient.PutAsJsonAsync($"products/{Product.Id}", Product);
            response.EnsureSuccessStatusCode();
            await MessageBox.ShowDialogAsync("Success", "Product updated successfully");
            _navigationService.NavigateTo<ProductsTableViewModel>();
        }
        catch (Exception e)
        {
            await MessageBox.ShowDialogAsync("Error", e.Message);
        }
    }
    
    [RelayCommand]
    private void Cancel()
    {
        _navigationService.NavigateTo<ProductsTableViewModel>();
    }
}