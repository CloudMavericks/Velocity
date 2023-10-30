using System.Net.Http;
using System.Net.Http.Json;
using Velocity.Shared.Requests.Suppliers;
using Velocity.Shared.Responses.Suppliers;
using Velocity.Shared.Wrapper;

namespace Velocity.Frontend.Services.HttpClients;

public class SupplierHttpClient
{
    private readonly HttpClient _httpClient;

    public SupplierHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<PaginatedResult<SupplierResponse>> GetAllAsync(int pageNumber, int pageSize, string search, CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<PaginatedResult<SupplierResponse>>($"/suppliers?pageNumber={pageNumber}&pageSize={pageSize}&searchString={search}", cancellationToken: cancellationToken);
    }
    
    public Task<SupplierResponse> GetAsync(Guid id)
    {
        return _httpClient.GetFromJsonAsync<SupplierResponse>($"/suppliers/{id}");
    }
    
    public async Task CreateAsync(CreateSupplierRequest supplier)
    {
        var response = await _httpClient.PostAsJsonAsync("/suppliers", supplier);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UpdateAsync(UpdateSupplierRequest supplier)
    {
        var response = await _httpClient.PutAsJsonAsync($"/suppliers/{supplier.Id}", supplier);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/suppliers/{id}");
        response.EnsureSuccessStatusCode();
    }
}