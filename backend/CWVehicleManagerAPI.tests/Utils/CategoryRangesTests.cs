using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Utils;

namespace CWVehicleManagerAPI.tests.Utils;

public class CategoryRangesTests
{
    [Fact]
    public void SetUpperLimits_SetsCorrectRangesForAllCategories()
    {
        
        var categories = new List<VehicleCategory>
        {
            new()
            {
                Name = "Light",
                MinWeightKg = 0m
            },
            new()
            {
                Name = "Medium",
                MinWeightKg = 500m
            },
            new()
            {
                Name = "Heavy",
                MinWeightKg = 2500m
            }
        };

        
        CategoryRanges.SetUpperLimits(categories);

        
        Assert.Equal(500m, categories[0].MaxWeightKg);
        Assert.Equal(2500m, categories[1].MaxWeightKg);
        Assert.Null(categories[2].MaxWeightKg);
    }

    [Fact]
    public void SetUpperLimits_WhenCategoriesAreUnordered_StillSetsCorrectRanges()
    {
        
        var categories = new List<VehicleCategory>
        {
            new()
            {
                Name = "Heavy",
                MinWeightKg = 2500m
            },
            new()
            {
                Name = "Light",
                MinWeightKg = 0m
            },
            new()
            {
                Name = "Medium",
                MinWeightKg = 500m
            }
        };

        
        CategoryRanges.SetUpperLimits(categories);

        
        var light = categories.Single(c => c.Name == "Light");
        var medium = categories.Single(c => c.Name == "Medium");
        var heavy = categories.Single(c => c.Name == "Heavy");

        Assert.Equal(500m, light.MaxWeightKg);
        Assert.Equal(2500m, medium.MaxWeightKg);
        Assert.Null(heavy.MaxWeightKg);
    }

    [Fact]
    public void SetUpperLimits_WhenNewCategoryIsAdded_AdjustsNeighbouringRanges()
    {
        
        var categories = new List<VehicleCategory>
        {
            new()
            {
                Name = "Light",
                MinWeightKg = 0m
            },
            new()
            {
                Name = "Medium",
                MinWeightKg = 500m
            },
            new()
            {
                Name = "Large",
                MinWeightKg = 2000m
            },
            new()
            {
                Name = "Heavy",
                MinWeightKg = 2500m
            }
        };

        
        CategoryRanges.SetUpperLimits(categories);

        
        var medium = categories.Single(c => c.Name == "Medium");
        var large = categories.Single(c => c.Name == "Large");

        Assert.Equal(2000m, medium.MaxWeightKg);
        Assert.Equal(2500m, large.MaxWeightKg);
    }

    [Fact]
    public void SetUpperLimits_WhenCategoryStartChanges_AdjustsPreviousCategoryEnd()
    {
        
        var categories = new List<VehicleCategory>
        {
            new()
            {
                Name = "Light",
                MinWeightKg = 0m
            },
            new()
            {
                Name = "Medium",
                MinWeightKg = 500m
            },
            new()
            {
                Name = "Heavy",
                MinWeightKg = 2000m
            }
        };

        
        CategoryRanges.SetUpperLimits(categories);

        
        var medium = categories.Single(c => c.Name == "Medium");

        Assert.Equal(2000m, medium.MaxWeightKg);
    }

    [Fact]
    public void SetUpperLimits_HeaviestCategoryHasNoUpperLimit()
    {
        
        var categories = new List<VehicleCategory>
        {
            new()
            {
                Name = "Light",
                MinWeightKg = 0m
            },
            new()
            {
                Name = "Heavy",
                MinWeightKg = 2500m
            }
        };

        
        CategoryRanges.SetUpperLimits(categories);

        
        var heavy = categories.Single(c => c.Name == "Heavy");

        Assert.Null(heavy.MaxWeightKg);
    }
}