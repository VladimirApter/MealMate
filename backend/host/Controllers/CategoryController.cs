using host.DataBase;
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
}