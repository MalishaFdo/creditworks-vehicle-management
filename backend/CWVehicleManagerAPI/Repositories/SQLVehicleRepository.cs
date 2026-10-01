using CWVehicleManagerAPI.Data;
using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CWVehicleManagerAPI.Repositories;

public class SQLVehicleRepository : IVehicleRepository
{
    private readonly AppDbContext dbContext;

    public SQLVehicleRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Vehicle>> GetAllAsync(VehicleSortField sortBy, SortDirection sortDirection)
    {
        var vehicles = dbContext.Vehicles.AsNoTracking().Include(v => v.Manufacturer);
        var descending = sortDirection == SortDirection.Desc;

       
        var sorted = sortBy switch
        {
            VehicleSortField.Manufacturer => descending
                ? vehicles.OrderByDescending(v => v.Manufacturer!.Name)
                : vehicles.OrderBy(v => v.Manufacturer!.Name),
            VehicleSortField.YearOfManufacture => descending
                ? vehicles.OrderByDescending(v => v.YearOfManufacture)
                : vehicles.OrderBy(v => v.YearOfManufacture),
            VehicleSortField.Weight => descending
                ? vehicles.OrderByDescending(v => v.WeightKg)
                : vehicles.OrderBy(v => v.WeightKg),
            _ => descending
                ? vehicles.OrderByDescending(v => v.OwnerName)
                : vehicles.OrderBy(v => v.OwnerName),
        };

       
        return await sorted.ThenBy(v => v.Id).ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await dbContext.Vehicles
            .AsNoTracking()
            .Include(v => v.Manufacturer)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Vehicle> CreateAsync(Vehicle vehicle)
    {
        await dbContext.Vehicles.AddAsync(vehicle);
        await dbContext.SaveChangesAsync();

        
        await dbContext.Entry(vehicle).Reference(v => v.Manufacturer).LoadAsync();
        return vehicle;
    }
}
