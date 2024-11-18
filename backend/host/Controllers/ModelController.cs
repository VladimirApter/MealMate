using host.DataBaseAccess;
using host.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace host.Controllers
{
    [ApiController]
    [Route("api")]
    public class ModelController : ControllerBase
    {
        private static readonly JsonSerializerOptions
            OptionsDeserializer = new() { PropertyNameCaseInsensitive = true };

        [HttpGet("{entityType}/{id}"), HttpPost("{entityType}")]
        public async Task<IActionResult> GetPostEntity(string entityType, int id, [FromBody] object? entity)
        {
            return entityType.ToLower() switch
            {
                "dish" => await GetPostEntity(id, DeserializeEntity<Dish>(entity)),
                "category" => await GetPostEntity(id, DeserializeEntity<Category>(entity)),
                "menu" => await GetPostEntity(id, DeserializeEntity<Menu>(entity)),
                "notificationgetter" => await GetPostEntity(id, DeserializeEntity<NotificationGetter>(entity)),
                "owner" => await GetPostEntity(id, DeserializeEntity<Owner>(entity)),
                "table" => await GetPostEntity(id, DeserializeEntity<Table>(entity)),
                "restaurant" => await GetPostEntity(id, DeserializeEntity<Restaurant>(entity)),
                //"orderitem" => await GetPostEntity(id, DeserializeEntity<OrderItem>(entity)),
                _ => BadRequest("Invalid entity type.")
            };

            T? DeserializeEntity<T>(object? obj) where T : class => obj == null
                ? null
                : JsonSerializer.Deserialize<T>(obj.ToString() ?? string.Empty, OptionsDeserializer);
        }

        private async Task<IActionResult> GetPostEntity<T>(int id, [FromBody] T? entity) where T : class, ITableDataBase
        {
            if (entity == null) return Ok((object?)await DataBaseAccess<T>.GetAsync(id) ?? NotFound());

            DataBaseAccess<T>.AddOrUpdate(entity);
            return Ok(entity.Id);
        }
    }
}