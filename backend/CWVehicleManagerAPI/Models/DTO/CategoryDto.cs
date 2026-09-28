namespace CWVehicleManagerAPI.Models.DTO;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal MinWeightKg { get; set; }
    public decimal? MaxWeightKg { get; set; }
    public string IconKey { get; set; } = string.Empty;
}