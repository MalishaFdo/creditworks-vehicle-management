using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CWVehicleManagerAPI.Utils;

public record ValidationError(string Field, string Message);

public static class ValidationErrorExtensions
{
    public static void AddErrors(this ModelStateDictionary modelState, IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
        {
            modelState.AddModelError(error.Field, error.Message);
        }
    }
}