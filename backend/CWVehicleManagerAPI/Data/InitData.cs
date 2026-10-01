using CWVehicleManagerAPI.Models.Domain;

namespace CWVehicleManagerAPI.Data;

public class InitData
{
    //Manufacturers added to the database
    public static List<Manufacturer> Manufacturers() =>
    [
        new() { Id = 1, Name = "Mazda" },
        new() { Id = 2, Name = "Mercedes" },
        new() { Id = 3, Name = "Honda" },
        new() { Id = 4, Name = "Ferrari" },
        new() { Id = 5, Name = "Toyota" },
    ];

    //Vehicle categories and their weight ranges
    public static List<VehicleCategory> Categories() =>
    [
        new() { Id = 1, Name = "Light", MinWeightKg = 0m, MaxWeightKg = 500m, IconKey = "motorcycle" },
        new() { Id = 2, Name = "Medium", MinWeightKg = 500m, MaxWeightKg = 2500m, IconKey = "car" },
        new() { Id = 3, Name = "Heavy", MinWeightKg = 2500m, MaxWeightKg = null, IconKey = "truck" },
    ];
}