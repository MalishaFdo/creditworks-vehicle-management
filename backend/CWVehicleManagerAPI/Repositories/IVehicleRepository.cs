using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Models.DTO;

namespace CWVehicleManagerAPI.Repositories;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetAllAsync(VehicleSortField sortBy, SortDirection sortDirection);
    Task<Vehicle?> GetByIdAsync(int id);
    Task<Vehicle> CreateAsync(Vehicle vehicle);
}