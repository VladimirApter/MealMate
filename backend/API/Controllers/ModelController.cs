using Domain.DataBaseAccess;
using Domain.Logic;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;
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
                "table" => await GetPostTable(id, DeserializeEntity<Table>(entity)),
                "restaurant" => await GetPostEntity(id, DeserializeEntity<Restaurant>(entity)),
                "drink" => await GetPostEntity(id, DeserializeEntity<Drink>(entity)),
                "geocoordinates" => await GetPostEntity(id, DeserializeEntity<GeoCoordinates>(entity)),
                "nutrients" => await GetPostEntity(id, DeserializeEntity<Nutrients>(entity)),
                "orderitem" => await GetPostEntity(id, DeserializeEntity<OrderItem>(entity)),
                "client" => await GetPostEntity(id, DeserializeEntity<Client>(entity)),
                "order" => await GetPostEntity(id, DeserializeEntity<Order>(entity)),
                _ => BadRequest("Invalid entity type.")
            };

            T? DeserializeEntity<T>(object? obj) where T : class
            {
                return obj == null
                    ? null
                    : JsonSerializer.Deserialize<T>(obj.ToString() ?? string.Empty, OptionsDeserializer);
            }
        }

        private async Task<IActionResult> GetPostEntity<T>(int id, [FromBody] T? entity) where T : class, ITableDataBase
        {
            if (entity == null)
            {
                var o = await DataBaseAccess<T>.GetAsync(id);
                return o == null ? NotFound() : Ok(o);
            }

            if (entity is Order order)
            {
                if (order.Id == null) return NotFound(order.Id);
                var o = await DataBaseAccess<T>.GetAsync(order.Id.Value);
                DataBaseAccess<T>.AddOrUpdate(entity);
                if (o == null) await ForwardToPythonServer.ForwardObject(order, "http://localhost:5059/order");
            }
            else
            {
                DataBaseAccess<T>.AddOrUpdate(entity);
            }

            return Ok(entity.Id);
        }


        private async Task<IActionResult> GetPostTable<T>(int id, [FromBody] T? entityTable) where T : Table
        {
            if (entityTable == null)
            {
                var o = await DataBaseAccess<T>.GetAsync(id);
                return o == null ? NotFound() : Ok(o);
            }

            var table = new Table(entityTable.Id, entityTable.RestaurantId, entityTable.Number);
            if (entityTable.Id == null)
            {
                DataBaseAccess<T>.AddOrUpdateTable(table, true);
                var tableNew = new Table(table.Id, table.RestaurantId, table.Number);
                DataBaseAccess<T>.AddOrUpdateTable(tableNew, true);
            }
            else
                DataBaseAccess<T>.AddOrUpdateTable(table, false);

            return Ok(table.Id);
        }

        [HttpDelete("{entityType}/{id}")]
        public async Task<IActionResult> DeleteEntity(string entityType, int id)
        {
            return entityType.ToLower() switch
            {
                "dish" => await DeleteEntity<Dish>(id),
                "category" => await DeleteEntity<Category>(id),
                "menu" => await DeleteEntity<Menu>(id),
                "notificationgetter" => await DeleteEntity<NotificationGetter>(id),
                "owner" => await DeleteEntity<Owner>(id),
                "table" => await DeleteEntity<Table>(id),
                "restaurant" => await DeleteEntity<Restaurant>(id),
                "drink" => await DeleteEntity<Drink>(id),
                "geocoordinates" => await DeleteEntity<GeoCoordinates>(id),
                "nutrients" => await DeleteEntity<Nutrients>(id),
                "orderitem" => await DeleteEntity<OrderItem>(id),
                "client" => await DeleteEntity<Client>(id),
                "order" => await DeleteEntity<Order>(id),
                _ => BadRequest("Invalid entity type.")
            };
        }

        private async Task<IActionResult> DeleteEntity<T>(int id) where T : class, ITableDataBase
        {
            var entity = await DataBaseAccess<T>.GetAsync(id);
            if (entity == null) return NotFound();

            DataBaseAccess<T>.Delete(id);
            return Ok();
        }
    }
}