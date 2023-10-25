using System.Text.Json;
using System.Text.Json.Serialization;
using Velocity.Shared.Wrapper;

namespace Velocity.Avalonia.Extensions;

public static class ResultExtensions
{
    public static async Task<IResult> ToResult(this HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var jsonDeserialize = JsonSerializer.Deserialize<Result>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return jsonDeserialize;
    }
    public static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var jsonDeserialize = JsonSerializer.Deserialize<Result<T>>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        });
        return jsonDeserialize;
    }
}
