using CWVehicleManagerAPI.Data;
using CWVehicleManagerAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CWVehicleManagerAPI.Repositories;

public class SQLManufacturerRepository : IManufacturerRepository
{
    private readonly AppDbContext dbContext;

    public SQLManufacturerRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Manufacturer>> GetAllAsync()
    {
        return await dbContext.Manufacturers.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await dbContext.Manufacturers.AnyAsync(m => m.Id == id);
    }
}