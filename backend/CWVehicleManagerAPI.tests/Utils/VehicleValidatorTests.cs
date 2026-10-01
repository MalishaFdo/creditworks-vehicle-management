using CWVehicleManagerAPI.Utils;

namespace CWVehicleManagerAPI.tests.Utils;

public class VehicleValidatorTests
{
    private const int CurrentYear = 2026;

    [Fact]
    public void Validate_WhenVehicleIsValid_ReturnsNoErrors()
    {
       
        string ownerName = "John Smith";
        int year = 2020;
        decimal weightKg = 1500.50m;

        
        var errors = VehicleValidator.Validate(
            ownerName,
            year,
            weightKg,
            CurrentYear);

        
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WhenOwnerNameIsEmpty_ReturnsOwnerNameError()
    {
        var errors = VehicleValidator.Validate(
            "",
            2020,
            1500m,
            CurrentYear);

        Assert.Contains(errors,
            error => error.Field == "ownerName");
    }

    [Fact]
    public void Validate_WhenYearIsBeforeEarliestYear_ReturnsYearError()
    {
        var errors = VehicleValidator.Validate(
            "John Smith",
            1800,
            1500m,
            CurrentYear);

        Assert.Contains(errors,
            error => error.Field == "yearOfManufacture");
    }

    [Fact]
    public void Validate_WhenYearIsInFuture_ReturnsYearError()
    {
        var errors = VehicleValidator.Validate(
            "John Smith",
            2027,
            1500m,
            CurrentYear);

        Assert.Contains(errors,
            error => error.Field == "yearOfManufacture");
    }

    [Fact]
    public void Validate_WhenWeightIsZero_ReturnsWeightError()
    {
        var errors = VehicleValidator.Validate(
            "John Smith",
            2020,
            0m,
            CurrentYear);

        Assert.Contains(errors,
            error => error.Field == "weightKg");
    }

    [Fact]
    public void Validate_WhenWeightIsNegative_ReturnsWeightError()
    {
        var errors = VehicleValidator.Validate(
            "John Smith",
            2020,
            -100m,
            CurrentYear);

        Assert.Contains(errors,
            error => error.Field == "weightKg");
    }

    [Fact]
    public void Validate_WhenWeightHasMoreThanTwoDecimalPlaces_ReturnsWeightError()
    {
        var errors = VehicleValidator.Validate(
            "John Smith",
            2020,
            1500.123m,
            CurrentYear);

        Assert.Contains(errors,
            error => error.Field == "weightKg");
    }

    [Fact]
    public void Validate_WhenWeightHasTwoDecimalPlaces_ReturnsNoWeightError()
    {
        var errors = VehicleValidator.Validate(
            "John Smith",
            2020,
            1500.75m,
            CurrentYear);

        Assert.DoesNotContain(errors,
            error => error.Field == "weightKg");
    }

    [Fact]
    public void Validate_WhenYearIsEarliestAllowedYear_ReturnsNoYearError()
    {
        var errors = VehicleValidator.Validate(
            "John Smith",
            VehicleValidator.EarliestYear,
            1500m,
            CurrentYear);

        Assert.DoesNotContain(errors,
            error => error.Field == "yearOfManufacture");
    }

    [Fact]
    public void Validate_WhenYearIsCurrentYear_ReturnsNoYearError()
    {
        var errors = VehicleValidator.Validate(
            "John Smith",
            CurrentYear,
            1500m,
            CurrentYear);

        Assert.DoesNotContain(errors,
            error => error.Field == "yearOfManufacture");
    }
}