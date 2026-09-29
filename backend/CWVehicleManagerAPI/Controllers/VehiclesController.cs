using CWVehicleManagerAPI.Mappings;
using CWVehicleManagerAPI.Models.DTO;
using CWVehicleManagerAPI.Repositories;
using CWVehicleManagerAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CWVehicleManagerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleRepository vehicleRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly IManufacturerRepository manufacturerRepository;

    public VehiclesController(
        IVehicleRepository vehicleRepository,
        ICategoryRepository categoryRepository,
        IManufacturerRepository manufacturerRepository)
    {
        this.vehicleRepository = vehicleRepository;
        this.categoryRepository = categoryRepository;
        this.manufacturerRepository = manufacturerRepository;
    }
    
    [HttpGet]
    [ProducesResponseType<List<VehicleDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] VehicleSortField sortBy = VehicleSortField.OwnerName,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc)
    {
        var vehicles = await vehicleRepository.GetAllAsync(sortBy, sortDirection);
        var categories = await categoryRepository.GetAllAsync();

        return Ok(vehicles.Select(v => v.ToDto(categories)));
    }

    /// <summary>Gets one vehicle with its current category.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType<VehicleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(id);
        if (vehicle == null)
        {
            return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Vehicle {id} was not found.");
        }

        var categories = await categoryRepository.GetAllAsync();
        return Ok(vehicle.ToDto(categories));
    }
    
    [HttpPost]
    [ProducesResponseType<VehicleDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AddVehicleDto addVehicleDto)
    {

        var vehicle = addVehicleDto.ToDomain();

        var errors = VehicleValidator.Validate(vehicle.OwnerName, vehicle.YearOfManufacture, vehicle.WeightKg, DateTime.Now.Year);
        if (!await manufacturerRepository.ExistsAsync(vehicle.ManufacturerId))
        {
            errors.Add(new ValidationError("manufacturerId", "Select a manufacturer from the list."));
        }

        if (errors.Count > 0)
        {
            ModelState.AddErrors(errors);
            return ValidationProblem(ModelState);
        }

        vehicle = await vehicleRepository.CreateAsync(vehicle);
        var categories = await categoryRepository.GetAllAsync();

        return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle.ToDto(categories));
    }
}
