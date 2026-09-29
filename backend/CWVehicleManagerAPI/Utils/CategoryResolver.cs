using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.Utils;

public static class CategoryResolver
{
    public static VehicleCategory? Resolve(IEnumerable<VehicleCategory> categories, decimal weightKg) =>
        categories.FirstOrDefault(category => category.Contains(weightKg));
}