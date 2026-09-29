using CWVehicleManagerAPI.Mappings;
using CWVehicleManagerAPI.Models.DTO;
using CWVehicleManagerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CWVehicleManagerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturersController : ControllerBase
{
    private readonly IManufacturerRepository manufacturerRepository;

    public ManufacturersController(IManufacturerRepository manufacturerRepository)
    {
        this.manufacturerRepository = manufacturerRepository;
    }
    
    [HttpGet]
    [ProducesResponseType<List<ManufacturerDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var manufacturers = await manufacturerRepository.GetAllAsync();
        return Ok(manufacturers.Select(m => m.ToDto()));
    }
}
