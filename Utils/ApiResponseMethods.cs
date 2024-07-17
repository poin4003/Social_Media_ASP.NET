using api.Helpers.ApiResponseObject;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Utils.ApiResponseMethod;
public static class ApiResponseMethod
{
    public static ApiResponseObject<T> ToApiResponseObject<T>(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        var message = string.Join("; ", errors);

        return new ApiResponseObject<T>
        {
            Record = default!,
            Message = $"Failed: {message}"
        };
    }
}