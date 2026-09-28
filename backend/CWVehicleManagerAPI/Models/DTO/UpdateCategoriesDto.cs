using System.ComponentModel.DataAnnotations;

namespace CWVehicleManagerAPI.Models.DTO;

public class UpdateCategoriesDto
{
    public int? Id { get; set; }
    
    [Required(ErrorMessage = "Category name is required.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Minimum weight is required.")]
    public decimal? MinWeightKg { get; set; }
    
    public decimal? MaxWeightKg { get; set; }

    [Required(ErrorMessage = "Category icon is required.")]
    public string? IconKey { get; set; }
}