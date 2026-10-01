using CWVehicleManagerAPI.Utils;

namespace CWVehicleManagerAPI.tests.Utils;

public class VehicleValidatorTests
{
    private const int CurrentYear = 2026;
    private const string ValidOwnerName = "John Smith";
    private const int ValidYear = 2020;
    private const decimal ValidWeightKg = 1500m;

    public static TheoryData<decimal, string> InvalidWeights => new()
    {
        { 0m, "greater than 0 kg" },
        { -100m, "greater than 0 kg" },
        { 1500.123m, "at most 2 decimal places" },
    };

    public static TheoryData<decimal> ValidWeights => new()
    {
        0.01m,
        1500.75m,
        1500.5m,
    };

    [Fact]
    public void Validate_WhenVehicleIsValid_ReturnsNoErrors()
    {
        var errors = VehicleValidator.Validate(
            ValidOwnerName,
            ValidYear,
            1500.50m,
            CurrentYear);

        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WhenOwnerNameIsEmpty_ReturnsOwnerNameError()
    {
        var errors = VehicleValidator.Validate(
            "",
            ValidYear,
            ValidWeightKg,
            CurrentYear);

        var error = Assert.Single(errors);
        Assert.Equal("ownerName", error.Field);
        Assert.Contains("required", error.Message);
    }

    [Theory]
    [InlineData(1800)]
    [InlineData(VehicleValidator.EarliestYear - 1)]
    [InlineData(CurrentYear + 1)]
    public void Validate_WhenYearIsOutOfRange_ReturnsYearError(int year)
    {
        var errors = VehicleValidator.Validate(
            ValidOwnerName,
            year,
            ValidWeightKg,
            CurrentYear);

        var error = Assert.Single(errors);
        Assert.Equal("yearOfManufacture", error.Field);
        Assert.Contains($"between {VehicleValidator.EarliestYear} and {CurrentYear}", error.Message);
    }

    [Theory]
    [InlineData(VehicleValidator.EarliestYear)]
    [InlineData(CurrentYear)]
    public void Validate_WhenYearIsWithinRange_ReturnsNoErrors(int year)
    {
        var errors = VehicleValidator.Validate(
            ValidOwnerName,
            year,
            ValidWeightKg,
            CurrentYear);

        Assert.Empty(errors);
    }

    [Theory]
    [MemberData(nameof(InvalidWeights))]
    public void Validate_WhenWeightIsInvalid_ReturnsWeightError(decimal weightKg, string expectedMessage)
    {
        var errors = VehicleValidator.Validate(
            ValidOwnerName,
            ValidYear,
            weightKg,
            CurrentYear);

        var error = Assert.Single(errors);
        Assert.Equal("weightKg", error.Field);
        Assert.Contains(expectedMessage, error.Message);
    }

    [Theory]
    [MemberData(nameof(ValidWeights))]
    public void Validate_WhenWeightIsValid_ReturnsNoErrors(decimal weightKg)
    {
        var errors = VehicleValidator.Validate(
            ValidOwnerName,
            ValidYear,
            weightKg,
            CurrentYear);

        Assert.Empty(errors);
    }
}
