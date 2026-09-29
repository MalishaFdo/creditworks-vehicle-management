using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.Repositories;

public interface IManufacturerRepository
{
    Task<List<Manufacturer>> GetAllAsync();
    Task<bool> ExistsAsync(int id);
}