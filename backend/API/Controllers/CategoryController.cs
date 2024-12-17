using Domain.DataBaseAccess;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(long id)
    {
        var o = await DataBaseAccess<Category>.GetAsync(id);
        return o == null ? NotFound("Category is null") : Ok(o);
    }
    
    [HttpPost]
    public IActionResult PostCategory([FromBody] Category? category)
    {
        if (category == null) return NotFound("Category is null");
        
        DataBaseAccess<Category>.AddOrUpdate(category);
        return Ok(category.Id);
    }
    [HttpDelete("{id}")]
    private async Task<IActionResult> DeleteCategory(long id)
    {
        var category = await DataBaseAccess<Category>.GetAsync(id);
        if (category == null) return NotFound("Category is null");

        DataBaseAccess<Category>.Delete(id);
        return Ok();
    }
}