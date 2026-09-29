namespace CWVehicleManagerAPI.Utils;

public static class VehicleValidator
{
    public const int MaxOwnerNameLength = 100;
    
    public const int EarliestYear = 1886;

    public static List<ValidationError> Validate(string ownerName, int yearOfManufacture, decimal weightKg, int currentYear)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(ownerName))
        {
            errors.Add(new("ownerName", "Owner name is required."));
        }
        else if (ownerName.Trim().Length > MaxOwnerNameLength)
        {
            errors.Add(new("ownerName", $"Owner name must not be exceed {MaxOwnerNameLength} characters."));
        }
        
        if (yearOfManufacture < EarliestYear || yearOfManufacture > currentYear)
        {
            errors.Add(new("yearOfManufacture", $"Year of manufacture must be between {EarliestYear} and {currentYear}."));
        }

        if (weightKg <= 0)
        {
            errors.Add(new("weightKg", "Weight must be greater than 0 kg."));
        }
        else if (weightKg > WeightRules.MaxWeightKg)
        {
            errors.Add(new("weightKg", "Weight cannot exceed 50,000 kg."));
        }

        if (!WeightRules.HasTwoDecimalPlaces(weightKg))
        {
            errors.Add(new("weightKg", "Weight can have at most 2 decimal places."));
        }

        return errors;
    }
}
