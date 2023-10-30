using System.Net.Http;
using System.Net.Http.Json;
using Velocity.Frontend.Extensions;
using Velocity.Shared.Requests.Products;
using Velocity.Shared.Responses.Products;
using Velocity.Shared.Wrapper;

namespace Velocity.Frontend.Services.HttpClients;

public class ProductHttpClient
{
    private readonly HttpClient _httpClient;

    public ProductHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<PaginatedResult<ProductResponse>> GetAllAsync(int pageNumber, int pageSize, string search, CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<PaginatedResult<ProductResponse>>($"/products?pageNumber={pageNumber}&pageSize={pageSize}&searchString={search}", cancellationToken);
    }
    
    public async Task<PaginatedResult<ProductResponse>> GetAllAsync(GetProductsRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/products/get", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.ToPaginatedResult<ProductResponse>(cancellationToken);
    }
    
    public Task<ProductResponse> GetAsync(Guid id)
    {
        return _httpClient.GetFromJsonAsync<ProductResponse>($"/products/{id}");
    }
    
    public async Task CreateAsync(CreateProductRequest product)
    {
        var response = await _httpClient.PostAsJsonAsync("/products", product);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UpdateAsync(UpdateProductRequest product)
    {
        var response = await _httpClient.PutAsJsonAsync($"/products/{product.Id}", product);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/products/{id}");
        response.EnsureSuccessStatusCode();
    }
}