using System.ComponentModel.DataAnnotations;

namespace CWVehicleManagerAPI.Models.DTO;

public class SaveCategoryDto
{
    [Required(ErrorMessage = "Category name is required.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Category icon is required.")]
    public string? IconKey { get; set; }

    /// <summary>The weight this category starts at (included), in kg.</summary>
    [Required(ErrorMessage = "Start weight is required.")]
    public decimal? MinWeightKg { get; set; }
}