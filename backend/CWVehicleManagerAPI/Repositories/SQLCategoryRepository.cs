using System.Data;
using CWVehicleManagerAPI.Data;
using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace CWVehicleManagerAPI.Repositories;

public class SQLCategoryRepository : ICategoryRepository
{
    private readonly AppDbContext dbContext;

    public SQLCategoryRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<VehicleCategory>> GetAllAsync()
    {
        return await dbContext.VehicleCategories.AsNoTracking().OrderBy(c => c.MinWeightKg).ToListAsync();
    }

    public async Task<VehicleCategory?> GetByIdAsync(int id)
    {
        return await dbContext.VehicleCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<VehicleCategory> CreateAsync(VehicleCategory category)
    {
        
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var categories = await dbContext.VehicleCategories.ToListAsync();
        categories.Add(category);
        dbContext.VehicleCategories.Add(category);

        
        CategoryRanges.SetUpperLimits(categories);

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return category;
    }

    public async Task<VehicleCategory?> UpdateAsync(int id, VehicleCategory category)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var categories = await dbContext.VehicleCategories.ToListAsync();
        var existing = categories.FirstOrDefault(c => c.Id == id);
        if (existing == null)
        {
            return null;
        }

        existing.Name = category.Name;
        existing.IconKey = category.IconKey;
        existing.MinWeightKg = category.MinWeightKg;

        
        CategoryRanges.SetUpperLimits(categories);

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return existing;
    }

    public async Task<VehicleCategory?> DeleteAsync(int id)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var categories = await dbContext.VehicleCategories.ToListAsync();
        var existing = categories.FirstOrDefault(c => c.Id == id);
        if (existing == null)
        {
            return null;
        }

        dbContext.VehicleCategories.Remove(existing);
        categories.Remove(existing);
        await dbContext.SaveChangesAsync();

        
        if (existing.MinWeightKg == 0 && categories.Count > 0)
        {
            categories.OrderBy(c => c.MinWeightKg).First().MinWeightKg = 0;
        }

        CategoryRanges.SetUpperLimits(categories);

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return existing;
    }
}
