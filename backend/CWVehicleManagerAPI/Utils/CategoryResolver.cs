using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.Utils;

public static class CategoryResolver
{
    // Find the category that matches the vehicle's weight
    public static VehicleCategory? Resolve(IEnumerable<VehicleCategory> categories, decimal weightKg) =>
        categories.FirstOrDefault(category => category.Contains(weightKg));
}