namespace CWVehicleManagerAPI.Models.DTO;

public class VehicleDto
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public int ManufacturerId { get; set; }
    public string ManufacturerName { get; set; } = string.Empty;
    public int YearOfManufacture { get; set; }
    public decimal WeightKg { get; set; }
    public CategoryDto? Category { get; set; }
}