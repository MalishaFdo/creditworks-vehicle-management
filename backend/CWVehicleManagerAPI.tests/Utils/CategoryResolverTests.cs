using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Utils;

namespace CWVehicleManagerAPI.tests.Utils;

public class CategoryResolverTests
{
    private readonly List<VehicleCategory> _categories =
    [
        new VehicleCategory
        {
            Id = 1,
            Name = "Light",
            MinWeightKg = 0m,
            MaxWeightKg = 500m,
            IconKey = "light"
        },
        new VehicleCategory
        {
            Id = 2,
            Name = "Medium",
            MinWeightKg = 500m,
            MaxWeightKg = 2500m,
            IconKey = "medium"
        },
        new VehicleCategory
        {
            Id = 3,
            Name = "Heavy",
            MinWeightKg = 2500m,
            MaxWeightKg = null,
            IconKey = "heavy"
        }
    ];

    [Fact]
    public void Resolve_WhenWeightIs300_ReturnsLight()
    {
        var result = CategoryResolver.Resolve(_categories, 300m);

        Assert.NotNull(result);
        Assert.Equal("Light", result.Name);
    }

    [Fact]
    public void Resolve_WhenWeightIs1500_ReturnsMedium()
    {
        var result = CategoryResolver.Resolve(_categories, 1500m);

        Assert.NotNull(result);
        Assert.Equal("Medium", result.Name);
    }

    [Fact]
    public void Resolve_WhenWeightIs3000_ReturnsHeavy()
    {
        var result = CategoryResolver.Resolve(_categories, 3000m);

        Assert.NotNull(result);
        Assert.Equal("Heavy", result.Name);
    }

    [Fact]
    public void Resolve_WhenWeightIsExactly500_ReturnsMedium()
    {
        var result = CategoryResolver.Resolve(_categories, 500m);

        Assert.NotNull(result);
        Assert.Equal("Medium", result.Name);
    }

    [Fact]
    public void Resolve_WhenWeightIsExactly2500_ReturnsHeavy()
    {
        var result = CategoryResolver.Resolve(_categories, 2500m);

        Assert.NotNull(result);
        Assert.Equal("Heavy", result.Name);
    }
}