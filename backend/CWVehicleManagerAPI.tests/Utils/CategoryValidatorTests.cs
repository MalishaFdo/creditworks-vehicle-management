using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Utils;

namespace CWVehicleManagerAPI.tests.Utils;

public class CategoryValidatorTests
{
    [Fact]
    public void Validate_WhenNameIsEmpty_ReturnsNameRequiredError()
    {
        var category = new VehicleCategory
        {
            Name = "",
            MinWeightKg = 0m,
            MaxWeightKg = 500m,
            IconKey = "light"
        };

        var errors = CategoryValidator.Validate(
            category,
            new List<VehicleCategory>());

        Assert.Contains(errors,
            error => error.Field == "name");
    }

    [Fact]
    public void Validate_WhenCategoryNameAlreadyExists_ReturnsError()
    {
        var existingCategories = new List<VehicleCategory>
        {
            new VehicleCategory
            {
                Name = "Light",
                MinWeightKg = 0m,
                MaxWeightKg = 500m,
                IconKey = "light"
            }
        };

        var category = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = 500m,
            MaxWeightKg = 2500m,
            IconKey = "medium"
        };

        var errors = CategoryValidator.Validate(
            category,
            existingCategories);

        Assert.Contains(errors,
            error => error.Field == "name");
    }

    [Fact]
    public void Validate_WhenIconIsEmpty_ReturnsIconRequiredError()
    {
        var category = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = 0m,
            MaxWeightKg = 500m,
            IconKey = ""
        };

        var errors = CategoryValidator.Validate(
            category,
            new List<VehicleCategory>());

        Assert.Contains(errors,
            error => error.Field == "iconKey");
    }

    [Fact]
    public void Validate_WhenStartWeightIsNegative_ReturnsError()
    {
        var category = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = -1m,
            MaxWeightKg = 500m,
            IconKey = "light"
        };

        var errors = CategoryValidator.Validate(
            category,
            new List<VehicleCategory>());

        Assert.Contains(errors,
            error => error.Field == "minWeightKg");
    }

    [Fact]
    public void Validate_WhenTwoCategoriesHaveSameStartWeight_ReturnsError()
    {
        var existingCategories = new List<VehicleCategory>
        {
            new VehicleCategory
            {
                Name = "Light",
                MinWeightKg = 0m,
                MaxWeightKg = 500m,
                IconKey = "light"
            }
        };

        var category = new VehicleCategory
        {
            Name = "Another Category",
            MinWeightKg = 0m,
            MaxWeightKg = 1000m,
            IconKey = "medium"
        };

        var errors = CategoryValidator.Validate(
            category,
            existingCategories);

        Assert.Contains(errors,
            error => error.Field == "minWeightKg");
    }

    [Fact]
    public void Validate_WhenFirstCategoryDoesNotStartAtZero_ReturnsError()
    {
        var category = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = 100m,
            MaxWeightKg = 500m,
            IconKey = "light"
        };

        var errors = CategoryValidator.Validate(
            category,
            new List<VehicleCategory>());

        Assert.Contains(errors,
            error => error.Field == "minWeightKg");
    }
}