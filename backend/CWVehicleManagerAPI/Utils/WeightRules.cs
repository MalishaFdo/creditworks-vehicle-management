namespace CWVehicleManagerAPI.Utils;

public static class WeightRules
{
    public const decimal MaxWeightKg = 50_000m;
    
    public static bool HasTwoDecimalPlaces(decimal value) => decimal.Round(value, 2) == value;
}