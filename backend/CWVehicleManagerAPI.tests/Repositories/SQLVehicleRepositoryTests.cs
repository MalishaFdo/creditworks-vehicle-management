using CWVehicleManagerAPI.Data;
using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Models.DTO;
using CWVehicleManagerAPI.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CWVehicleManagerAPI.tests.Repositories;

public class SQLVehicleRepositoryTests : IAsyncLifetime
{
    private SqliteConnection _connection = null!;
    private AppDbContext _context = null!;
    private SQLVehicleRepository _repository = null!;

    public async Task InitializeAsync()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        await AddTestVehiclesAsync();

        _repository = new SQLVehicleRepository(_context);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

     private async Task AddTestVehiclesAsync()
    {
        var vehicles = new List<Vehicle>
        {
            new()
            {
                OwnerName = "Charlie",
                ManufacturerId = 1,
                YearOfManufacture = 2020,
                WeightKg = 2000m
            },

            new()
            {
                OwnerName = "Alice",
                ManufacturerId = 2,
                YearOfManufacture = 2018,
                WeightKg = 1000m
            },

            new()
            {
                OwnerName = "Bob",
                ManufacturerId = 3,
                YearOfManufacture = 2022,
                WeightKg = 3000m
            }
        };

        await _context.Vehicles.AddRangeAsync(vehicles);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_SortByOwnerNameAscending_ReturnsCorrectOrder()
    {
        var result = await _repository.GetAllAsync(
            VehicleSortField.OwnerName,
            SortDirection.Asc);

        Assert.Equal(
            new[] { "Alice", "Bob", "Charlie" },
            result.Select(v => v.OwnerName));
    }

    [Fact]
    public async Task GetAllAsync_SortByOwnerNameDescending_ReturnsCorrectOrder()
    {
        var result = await _repository.GetAllAsync(
            VehicleSortField.OwnerName,
            SortDirection.Desc);

        Assert.Equal(
            new[] { "Charlie", "Bob", "Alice" },
            result.Select(v => v.OwnerName));
    }

    [Fact]
    public async Task GetAllAsync_SortByYearAscending_ReturnsCorrectOrder()
    {
        var result = await _repository.GetAllAsync(
            VehicleSortField.YearOfManufacture,
            SortDirection.Asc);

        Assert.Equal(
            new[] { 2018, 2020, 2022 },
            result.Select(v => v.YearOfManufacture));
    }

    [Fact]
    public async Task GetAllAsync_SortByWeightAscending_ReturnsCorrectOrder()
    {
        var result = await _repository.GetAllAsync(
            VehicleSortField.Weight,
            SortDirection.Asc);

        Assert.Equal(
            new[] { 1000m, 2000m, 3000m },
            result.Select(v => v.WeightKg));
    }

    [Fact]
    public async Task GetAllAsync_SortByWeightDescending_ReturnsCorrectOrder()
    {
        var result = await _repository.GetAllAsync(
            VehicleSortField.Weight,
            SortDirection.Desc);

        Assert.Equal(
            new[] { 3000m, 2000m, 1000m },
            result.Select(v => v.WeightKg));
    }

    [Fact]
    public async Task GetAllAsync_SortByManufacturerAscending_ReturnsCorrectOrder()
    {
        var result = await _repository.GetAllAsync(
            VehicleSortField.Manufacturer,
            SortDirection.Asc);

        Assert.Equal(
            new[] { "Honda", "Mazda", "Mercedes" },
            result.Select(v => v.Manufacturer!.Name));
    }
}
