using CWVehicleManagerAPI.Mappings;
using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.tests.Mappings;

public class MappingExtensionsTests
{
    private readonly Vehicle _vehicle = new()
    {
        Id = 1,
        OwnerName = "John Smith",
        ManufacturerId = 1,
        YearOfManufacture = 2020,
        WeightKg = 2200m
    };

    private static List<VehicleCategory> CreateCategories(decimal heavyStartKg) =>
    [
        new()
        {
            Id = 1,
            Name = "Light",
            MinWeightKg = 0m,
            MaxWeightKg = 500m,
            IconKey = "motorcycle"
        },
        new()
        {
            Id = 2,
            Name = "Medium",
            MinWeightKg = 500m,
            MaxWeightKg = heavyStartKg,
            IconKey = "car"
        },
        new()
        {
            Id = 3,
            Name = "Heavy",
            MinWeightKg = heavyStartKg,
            MaxWeightKg = null,
            IconKey = "truck"
        }
    ];

    [Fact]
    public void ToDto_WithOriginalRanges_UsesMediumCategory()
    {
        var dto = _vehicle.ToDto(CreateCategories(heavyStartKg: 2500m));

        Assert.NotNull(dto.Category);
        Assert.Equal("Medium", dto.Category.Name);
    }

    [Fact]
    public void ToDto_WhenHeavyStartsBelowVehicleWeight_UsesHeavyCategory()
    {
        var dto = _vehicle.ToDto(CreateCategories(heavyStartKg: 2000m));

        Assert.NotNull(dto.Category);
        Assert.Equal("Heavy", dto.Category.Name);
    }
}
