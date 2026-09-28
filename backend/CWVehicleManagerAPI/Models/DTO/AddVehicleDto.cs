using System.ComponentModel.DataAnnotations;

namespace CWVehicleManagerAPI.Models.DTO;

public class AddVehicleDto
{
    [Required(ErrorMessage = "Owner's name is required.")]
    public string? OwnerName { get; set; }

    [Required(ErrorMessage = "Manufacturer is required.")]
    public int? ManufacturerId { get; set; }

    [Required(ErrorMessage = "Year of manufacture is required.")]
    public int? YearOfManufacture { get; set; }

    [Required(ErrorMessage = "Weight is required.")]
    public decimal? WeightKg { get; set; }
}