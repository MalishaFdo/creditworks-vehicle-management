using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.Utils;

public static class CategoryRanges
{
    public static void SetUpperLimits(List<VehicleCategory> categories)
    {
        var sorted = categories.OrderBy(c => c.MinWeightKg).ToList();

        for (var i = 0; i < sorted.Count; i++)
        {
            var isHeaviest = i == sorted.Count - 1;
            sorted[i].MaxWeightKg = isHeaviest ? null : sorted[i + 1].MinWeightKg;
        }
    }
}