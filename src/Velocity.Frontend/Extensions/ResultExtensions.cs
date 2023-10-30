using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Velocity.Shared.Wrapper;

namespace Velocity.Frontend.Extensions;

public static class ResultExtensions
{
    public static async Task<IResult> ToResult(this HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken = default)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        var jsonDeserialize = JsonSerializer.Deserialize<Result>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return jsonDeserialize;
    }
    public static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken = default)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        var jsonDeserialize = JsonSerializer.Deserialize<Result<T>>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return jsonDeserialize;
    }
    
    public static async Task<PaginatedResult<T>> ToPaginatedResult<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        var responseAsString = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseObject = JsonSerializer.Deserialize<PaginatedResult<T>>(responseAsString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return responseObject;
    }
}