using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Utils;

namespace CWVehicleManagerAPI.tests.Utils;

public class CategoryValidatorTests
{
    [Fact]
    public void Validate_WhenCategoryIsValid_ReturnsNoErrors()
    {
        var existingCategories = new List<VehicleCategory>
        {
            new VehicleCategory
            {
                Name = "Light",
                MinWeightKg = 0m,
                MaxWeightKg = 500m,
                IconKey = "motorcycle"
            }
        };

        var category = new VehicleCategory
        {
            Name = "Medium",
            MinWeightKg = 500m,
            IconKey = "car"
        };

        var errors = CategoryValidator.Validate(
            category,
            existingCategories);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WhenNameIsEmpty_ReturnsNameRequiredError()
    {
        var category = new VehicleCategory
        {
            Name = "",
            MinWeightKg = 0m,
            MaxWeightKg = 500m,
            IconKey = "motorcycle"
        };

        var errors = CategoryValidator.Validate(
            category,
            new List<VehicleCategory>());

        var error = Assert.Single(errors);
        Assert.Equal("name", error.Field);
        Assert.Contains("Category name is required", error.Message);
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
                IconKey = "motorcycle"
            }
        };

        var category = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = 500m,
            MaxWeightKg = 2500m,
            IconKey = "car"
        };

        var errors = CategoryValidator.Validate(
            category,
            existingCategories);

        var error = Assert.Single(errors);
        Assert.Equal("name", error.Field);
        Assert.Contains("already a category called 'Light'", error.Message);
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

        var error = Assert.Single(errors);
        Assert.Equal("iconKey", error.Field);
        Assert.Contains("Category icon is required", error.Message);
    }

    [Fact]
    public void Validate_WhenIconIsUnknown_ReturnsIconError()
    {
        var category = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = 0m,
            MaxWeightKg = 500m,
            IconKey = "spaceship"
        };

        var errors = CategoryValidator.Validate(
            category,
            new List<VehicleCategory>());

        var error = Assert.Single(errors);
        Assert.Equal("iconKey", error.Field);
        Assert.Contains("'spaceship' is not a known icon", error.Message);
    }

    [Fact]
    public void Validate_WhenStartWeightIsNegative_ReturnsError()
    {
        var category = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = -1m,
            MaxWeightKg = 500m,
            IconKey = "motorcycle"
        };

        var errors = CategoryValidator.Validate(
            category,
            new List<VehicleCategory>());

        var error = Assert.Single(errors);
        Assert.Equal("minWeightKg", error.Field);
        Assert.Contains("cannot be negative", error.Message);
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
                IconKey = "motorcycle"
            }
        };

        var category = new VehicleCategory
        {
            Name = "Another Category",
            MinWeightKg = 0m,
            MaxWeightKg = 1000m,
            IconKey = "car"
        };

        var errors = CategoryValidator.Validate(
            category,
            existingCategories);

        var error = Assert.Single(errors);
        Assert.Equal("minWeightKg", error.Field);
        Assert.Contains("Two categories cannot start at the same weight", error.Message);
    }

    [Fact]
    public void Validate_WhenFirstCategoryDoesNotStartAtZero_ReturnsError()
    {
        var category = new VehicleCategory
        {
            Name = "Light",
            MinWeightKg = 100m,
            MaxWeightKg = 500m,
            IconKey = "motorcycle"
        };

        var errors = CategoryValidator.Validate(
            category,
            new List<VehicleCategory>());

        var error = Assert.Single(errors);
        Assert.Equal("minWeightKg", error.Field);
        Assert.Contains("must start at 0 kg", error.Message);
    }
}
