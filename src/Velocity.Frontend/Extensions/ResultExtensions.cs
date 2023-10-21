using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Velocity.Shared.Wrapper;

namespace Velocity.Frontend.Extensions;

public static class ResultExtensions
{
    public static async Task<IResult> ToResult(this HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var jsonDeserialize = JsonSerializer.Deserialize<Result>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        });
        return jsonDeserialize;
    }
    public static async Task<IResult<T>> ToResult<T>(this HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var jsonDeserialize = JsonSerializer.Deserialize<Result<T>>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        });
        return jsonDeserialize;
    }
}
