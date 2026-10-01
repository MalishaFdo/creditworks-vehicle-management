using CWVehicleManagerAPI.Data;
using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Models.DTO;
using CWVehicleManagerAPI.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CWVehicleManagerAPI.tests.Repositories;

public class SQLVehicleRepositoryTests
{
    private async Task<(AppDbContext Context, SqliteConnection Connection)>
        CreateDatabaseAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        return (context, connection);
    }

    private async Task AddTestVehiclesAsync(AppDbContext context)
    {
        
        var manufacturers = await context.Manufacturers
            .OrderBy(m => m.Id)
            .Take(3)
            .ToListAsync();

        var vehicles = new List<Vehicle>
        {
            new()
            {
                OwnerName = "Charlie",
                ManufacturerId = manufacturers[0].Id,
                YearOfManufacture = 2020,
                WeightKg = 2000m
            },

            new()
            {
                OwnerName = "Alice",
                ManufacturerId = manufacturers[1].Id,
                YearOfManufacture = 2018,
                WeightKg = 1000m
            },

            new()
            {
                OwnerName = "Bob",
                ManufacturerId = manufacturers[2].Id,
                YearOfManufacture = 2022,
                WeightKg = 3000m
            }
        };

        await context.Vehicles.AddRangeAsync(vehicles);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_SortByOwnerNameAscending_ReturnsCorrectOrder()
    {
        var (context, connection) = await CreateDatabaseAsync();

        try
        {
            await AddTestVehiclesAsync(context);

            var repository = new SQLVehicleRepository(context);

            var result = await repository.GetAllAsync(
                VehicleSortField.OwnerName,
                SortDirection.Asc);

            Assert.Equal(
                new[] { "Alice", "Bob", "Charlie" },
                result.Select(v => v.OwnerName));
        }
        finally
        {
            await context.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

    [Fact]
    public async Task GetAllAsync_SortByOwnerNameDescending_ReturnsCorrectOrder()
    {
        var (context, connection) = await CreateDatabaseAsync();

        try
        {
            await AddTestVehiclesAsync(context);

            var repository = new SQLVehicleRepository(context);

            var result = await repository.GetAllAsync(
                VehicleSortField.OwnerName,
                SortDirection.Desc);

            Assert.Equal(
                new[] { "Charlie", "Bob", "Alice" },
                result.Select(v => v.OwnerName));
        }
        finally
        {
            await context.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

    [Fact]
    public async Task GetAllAsync_SortByYearAscending_ReturnsCorrectOrder()
    {
        var (context, connection) = await CreateDatabaseAsync();

        try
        {
            await AddTestVehiclesAsync(context);

            var repository = new SQLVehicleRepository(context);

            var result = await repository.GetAllAsync(
                VehicleSortField.YearOfManufacture,
                SortDirection.Asc);

            Assert.Equal(
                new[] { 2018, 2020, 2022 },
                result.Select(v => v.YearOfManufacture));
        }
        finally
        {
            await context.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

    [Fact]
    public async Task GetAllAsync_SortByWeightAscending_ReturnsCorrectOrder()
    {
        var (context, connection) = await CreateDatabaseAsync();

        try
        {
            await AddTestVehiclesAsync(context);

            var repository = new SQLVehicleRepository(context);

            var result = await repository.GetAllAsync(
                VehicleSortField.Weight,
                SortDirection.Asc);

            Assert.Equal(
                new[] { 1000m, 2000m, 3000m },
                result.Select(v => v.WeightKg));
        }
        finally
        {
            await context.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

    [Fact]
    public async Task GetAllAsync_SortByWeightDescending_ReturnsCorrectOrder()
    {
        var (context, connection) = await CreateDatabaseAsync();

        try
        {
            await AddTestVehiclesAsync(context);

            var repository = new SQLVehicleRepository(context);

            var result = await repository.GetAllAsync(
                VehicleSortField.Weight,
                SortDirection.Desc);

            Assert.Equal(
                new[] { 3000m, 2000m, 1000m },
                result.Select(v => v.WeightKg));
        }
        finally
        {
            await context.DisposeAsync();
            await connection.DisposeAsync();
        }
    }

    [Fact]
    public async Task GetAllAsync_SortByManufacturerAscending_ReturnsCorrectOrder()
    {
        var (context, connection) = await CreateDatabaseAsync();

        try
        {
            await AddTestVehiclesAsync(context);

            var repository = new SQLVehicleRepository(context);

            var result = await repository.GetAllAsync(
                VehicleSortField.Manufacturer,
                SortDirection.Asc);

            var names = result
                .Select(v => v.Manufacturer!.Name)
                .ToList();

            var expected = names
                .OrderBy(name => name)
                .ToList();

            Assert.Equal(expected, names);
        }
        finally
        {
            await context.DisposeAsync();
            await connection.DisposeAsync();
        }
    }
}