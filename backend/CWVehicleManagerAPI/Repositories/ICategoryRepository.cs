using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.Repositories;

public interface ICategoryRepository
{
    Task<List<VehicleCategory>> GetAllAsync();

    Task<VehicleCategory?> GetByIdAsync(int id);

    Task<VehicleCategory> CreateAsync(VehicleCategory category);
    
    Task<VehicleCategory?> UpdateAsync(int id, VehicleCategory category);
    
    Task<VehicleCategory?> DeleteAsync(int id);
}