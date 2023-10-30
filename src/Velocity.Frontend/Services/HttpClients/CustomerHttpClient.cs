using System.Net.Http;
using System.Net.Http.Json;
using Velocity.Shared.Requests.Customers;
using Velocity.Shared.Responses.Customers;
using Velocity.Shared.Wrapper;

namespace Velocity.Frontend.Services.HttpClients;

public class CustomerHttpClient
{
    private readonly HttpClient _httpClient;

    public CustomerHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public Task<PaginatedResult<CustomerResponse>> GetAllAsync(int pageNumber, int pageSize, string search)
    {
        return _httpClient.GetFromJsonAsync<PaginatedResult<CustomerResponse>>($"/customers?pageNumber={pageNumber}&pageSize={pageSize}&searchString={search}");
    }
    
    public Task<CustomerResponse> GetAsync(Guid id)
    {
        return _httpClient.GetFromJsonAsync<CustomerResponse>($"/customers/{id}");
    }
    
    public async Task CreateAsync(CreateCustomerRequest customer)
    {
        var response = await _httpClient.PostAsJsonAsync("/customers", customer);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task UpdateAsync(UpdateCustomerRequest customer)
    {
        var response = await _httpClient.PutAsJsonAsync($"/customers/{customer.Id}", customer);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/customers/{id}");
        response.EnsureSuccessStatusCode();
    }
}