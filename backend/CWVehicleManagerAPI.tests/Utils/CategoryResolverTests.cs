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
            IconKey = "motorcycle"
        },
        new VehicleCategory
        {
            Id = 2,
            Name = "Medium",
            MinWeightKg = 500m,
            MaxWeightKg = 2500m,
            IconKey = "car"
        },
        new VehicleCategory
        {
            Id = 3,
            Name = "Heavy",
            MinWeightKg = 2500m,
            MaxWeightKg = null,
            IconKey = "truck"
        }
    ];

    public static TheoryData<decimal, string> WeightsWithinRange => new()
    {
        { 300m, "Light" },
        { 1500m, "Medium" },
        { 3000m, "Heavy" },
    };
    
    public static TheoryData<decimal, string> BoundaryWeights => new()
    {
        { 500m, "Medium" },
        { 2500m, "Heavy" },
    };

    [Theory]
    [MemberData(nameof(WeightsWithinRange))]
    public void Resolve_WhenWeightIsWithinRange_ReturnsMatchingCategory(decimal weightKg, string expectedCategory)
    {
        var result = CategoryResolver.Resolve(_categories, weightKg);

        Assert.NotNull(result);
        Assert.Equal(expectedCategory, result.Name);
    }

    [Theory]
    [MemberData(nameof(BoundaryWeights))]
    public void Resolve_WhenWeightIsOnBoundary_ReturnsHigherCategory(decimal weightKg, string expectedCategory)
    {
        var result = CategoryResolver.Resolve(_categories, weightKg);

        Assert.NotNull(result);
        Assert.Equal(expectedCategory, result.Name);
    }
}
