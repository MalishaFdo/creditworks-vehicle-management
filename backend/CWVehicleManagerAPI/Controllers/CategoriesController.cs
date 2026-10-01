using CWVehicleManagerAPI.Mappings;
using CWVehicleManagerAPI.Models.DTO;
using CWVehicleManagerAPI.Repositories;
using CWVehicleManagerAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CWVehicleManagerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }
    
    [HttpGet]
    [ProducesResponseType<List<CategoryDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var categories = await categoryRepository.GetAllAsync();
        return Ok(categories.Select(c => c.ToDto()));
    }
    //Get Category 
    [HttpGet("{id:int}")]
    [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return CategoryNotFound(id);
        }

        return Ok(category.ToDto());
    }
    //Create Category 
    [HttpPost]
    [ProducesResponseType<CategoryDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] SaveCategoryDto saveCategoryDto)
    {
        var category = saveCategoryDto.ToDomain();

        var errors = CategoryValidator.Validate(category, await categoryRepository.GetAllAsync());
        if (errors.Count > 0)
        {
            ModelState.AddErrors(errors);
            return ValidationProblem(ModelState);
        }

        category = await categoryRepository.CreateAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category.ToDto());
    }
    //Update Category
    [HttpPut("{id:int}")]
    [ProducesResponseType<CategoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SaveCategoryDto saveCategoryDto)
    {
        var categories = await categoryRepository.GetAllAsync();
        if (!categories.Any(c => c.Id == id))
        {
            return CategoryNotFound(id);
        }

        var category = saveCategoryDto.ToDomain();
        // Ignore the current category when checking for duplicate names
        var otherCategories = categories.Where(c => c.Id != id).ToList();

        var errors = CategoryValidator.Validate(category, otherCategories);
        if (errors.Count > 0)
        {
            ModelState.AddErrors(errors);
            return ValidationProblem(ModelState);
        }

        var updated = await categoryRepository.UpdateAsync(id, category);
        if (updated == null)
        {
            return CategoryNotFound(id);
        }

        return Ok(updated.ToDto());
    }
    //Delete Category 
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var categories = await categoryRepository.GetAllAsync();
        if (!categories.Any(c => c.Id == id))
        {
            return CategoryNotFound(id);
        }
        //Keeping remain at least 01 category   
        if (categories.Count == 1)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: "The last category cannot be deleted. Every vehicle must belong to a category.");
        }

        var deleted = await categoryRepository.DeleteAsync(id);
        if (deleted == null)
        {
            return CategoryNotFound(id);
        }

        return NoContent();
    }
    
    [HttpGet("icons")]
    [ProducesResponseType<List<string>>(StatusCodes.Status200OK)]
    public IActionResult GetIcons()
    {
        return Ok(CategoryIcons.All);
    }

    private ObjectResult CategoryNotFound(int id) =>
        Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Category {id} was not found.");
}
