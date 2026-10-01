using CWVehicleManagerAPI.Mappings;
using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.tests.Mappings;

public class MappingExtensionsTests
{
    [Fact]
    public void ToDto_WhenCategoryRangesChange_UsesCurrentCategory()
    {
        
        var vehicle = new Vehicle
        {
            Id = 1,
            OwnerName = "John Smith",
            ManufacturerId = 1,
            YearOfManufacture = 2020,
            WeightKg = 2200m
        };

        var originalCategories = new List<VehicleCategory>
        {
            new()
            {
                Id = 1,
                Name = "Light",
                MinWeightKg = 0m,
                MaxWeightKg = 500m,
                IconKey = "light"
            },
            new()
            {
                Id = 2,
                Name = "Medium",
                MinWeightKg = 500m,
                MaxWeightKg = 2500m,
                IconKey = "medium"
            },
            new()
            {
                Id = 3,
                Name = "Heavy",
                MinWeightKg = 2500m,
                MaxWeightKg = null,
                IconKey = "heavy"
            }
        };

        
        var originalDto = vehicle.ToDto(originalCategories);

        
        Assert.NotNull(originalDto.Category);
        Assert.Equal("Medium", originalDto.Category.Name);


        
        var updatedCategories = new List<VehicleCategory>
        {
            new()
            {
                Id = 1,
                Name = "Light",
                MinWeightKg = 0m,
                MaxWeightKg = 500m,
                IconKey = "light"
            },
            new()
            {
                Id = 2,
                Name = "Medium",
                MinWeightKg = 500m,
                MaxWeightKg = 2000m,
                IconKey = "medium"
            },
            new()
            {
                Id = 3,
                Name = "Heavy",
                MinWeightKg = 2000m,
                MaxWeightKg = null,
                IconKey = "heavy"
            }
        };

        
        var updatedDto = vehicle.ToDto(updatedCategories);

        
        Assert.NotNull(updatedDto.Category);
        Assert.Equal("Heavy", updatedDto.Category.Name);
    }
}