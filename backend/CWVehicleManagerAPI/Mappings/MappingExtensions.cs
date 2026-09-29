using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Models.DTO;
using CWVehicleManagerAPI.Utils;

namespace CWVehicleManagerAPI.Mappings;

public static class MappingExtensions
{
    public static ManufacturerDto ToDto(this Manufacturer manufacturer) => new()
    {
        Id = manufacturer.Id,
        Name = manufacturer.Name,
    };

    public static CategoryDto ToDto(this VehicleCategory category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        MinWeightKg = category.MinWeightKg,
        MaxWeightKg = category.MaxWeightKg,
        IconKey = category.IconKey,
    };
    
    public static VehicleDto ToDto(this Vehicle vehicle, List<VehicleCategory> categories) => new()
    {
        Id = vehicle.Id,
        OwnerName = vehicle.OwnerName,
        ManufacturerId = vehicle.ManufacturerId,
        ManufacturerName = vehicle.Manufacturer?.Name ?? string.Empty,
        YearOfManufacture = vehicle.YearOfManufacture,
        WeightKg = vehicle.WeightKg,
        Category = CategoryResolver.Resolve(categories, vehicle.WeightKg)?.ToDto(),
    };
    
    public static Vehicle ToDomain(this AddVehicleDto dto) => new()
    {
        OwnerName = dto.OwnerName!.Trim(),
        ManufacturerId = dto.ManufacturerId!.Value,
        YearOfManufacture = dto.YearOfManufacture!.Value,
        WeightKg = dto.WeightKg!.Value,
    };

    public static VehicleCategory ToDomain(this UpdateCategoryDto dto) => new()
    {
        Id = dto.Id ?? 0,
        Name = dto.Name?.Trim() ?? string.Empty,
        MinWeightKg = dto.MinWeightKg ?? 0m,
        MaxWeightKg = dto.MaxWeightKg,
        IconKey = dto.IconKey?.Trim() ?? string.Empty,
    };
    
    public static VehicleCategory ToDomain(this SaveCategoryDto dto) => new()
    {
        Name = dto.Name!.Trim(),
        IconKey = dto.IconKey!.Trim(),
        MinWeightKg = dto.MinWeightKg!.Value,
    };
}
