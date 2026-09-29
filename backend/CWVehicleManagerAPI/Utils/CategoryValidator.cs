using System.Globalization;
using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.Utils;

public static class CategoryValidator
{
    public const int MaxNameLength = 50;
    
    public static List<ValidationError> Validate(VehicleCategory category, List<VehicleCategory> otherCategories)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(category.Name))
        {
            errors.Add(new("name", "Category name is required."));
        }
        else if (category.Name.Length > MaxNameLength)
        {
            errors.Add(new("name", $"Category name must be {MaxNameLength} characters or fewer."));
        }
        else if (otherCategories.Any(c => string.Equals(c.Name, category.Name, StringComparison.OrdinalIgnoreCase)))
        {
            errors.Add(new("name", $"There is already a category called '{category.Name}'."));
        }

        if (string.IsNullOrWhiteSpace(category.IconKey))
        {
            errors.Add(new("iconKey", "Category icon is required."));
        }
        else if (!CategoryIcons.All.Contains(category.IconKey))
        {
            errors.Add(new("iconKey", $"'{category.IconKey}' is not a known icon."));
        }

        var start = category.MinWeightKg;
        if (start < 0)
        {
            errors.Add(new("minWeightKg", "Start weight cannot be negative."));
        }
        else if (start > WeightRules.MaxWeightKg)
        {
            errors.Add(new("minWeightKg", "Start weight cannot exceed 50,000 kg."));
        }
        else if (!WeightRules.HasTwoDecimalPlaces(start))
        {
            errors.Add(new("minWeightKg", "Start weight can have at most 2 decimal places."));
        }
        else
        {
            var sameStart = otherCategories.FirstOrDefault(c => c.MinWeightKg == start);
            if (sameStart != null)
            {
                // Two categories starting at the same weight would overlap.
                errors.Add(new("minWeightKg",
                    $"'{sameStart.Name}' already starts at {Kg(start)}. Two categories cannot start at the same weight."));
            }
            else if (start != 0 && !otherCategories.Any(c => c.MinWeightKg == 0))
            {
                // Nothing would cover the weights below this category.
                errors.Add(new("minWeightKg",
                    "This is the lightest category, so it must start at 0 kg. Otherwise lighter vehicles would have no category."));
            }
        }

        return errors;
    }

    private static string Kg(decimal value) => value.ToString("0.##", CultureInfo.InvariantCulture) + " kg";
}
