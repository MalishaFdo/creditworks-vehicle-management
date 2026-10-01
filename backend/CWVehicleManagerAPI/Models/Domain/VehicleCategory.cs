namespace CWVehicleManagerAPI.Models.Domain;

public class VehicleCategory
{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MinWeightKg { get; set; }
        public decimal? MaxWeightKg { get; set; }
        public string IconKey { get; set; } = string.Empty;
        
        // Check vehicle's weight belongs to this category
        public bool Contains(decimal weightKg) =>
            weightKg >= MinWeightKg && (MaxWeightKg == null || weightKg < MaxWeightKg);
}