using host.DataBaseAccess;
using host.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetCategory(int id)
    {
        var category = CategoriesAccess.GetCategory(id);
        if (category == null) return NotFound();

        return Ok(category);
    }
    
    [HttpPost]
    public IActionResult PostCategory([FromBody] Category? category)
    {
        if (category == null) return BadRequest("Category is null.");
        CategoriesAccess.AddOrUpdateCategory(category);
        
        return Ok(category.Id);
    }
}