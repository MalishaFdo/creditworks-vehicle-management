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
        // Serializable: if two people change categories at the same time, one waits for the other,
        // so their changes cannot mix into a set with gaps or overlaps.
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var categories = await dbContext.VehicleCategories.ToListAsync();
        categories.Add(category);
        dbContext.VehicleCategories.Add(category);

        // The new category ends where the next one starts, and the one below it now ends here.
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

        // Moving a start weight also moves where the category below it ends.
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

        // If the lightest category was deleted, the next one now starts at 0 kg.
        // Otherwise the category below simply stretches up to cover the deleted range.
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
