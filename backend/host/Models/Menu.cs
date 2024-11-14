using System.Collections.Generic;
using System.Text.Json.Serialization;
using host.DataBaseAccess;

namespace host.Models;

public class Menu : ITableDataBase
{
    public int? Id { get; set; }
    
    [JsonPropertyName("restaurant_id")]
    public int RestaurantId { get; set; }
    [JsonPropertyName("categories")]
    public List<Category>? Categories { get; set; }

    public Menu(int? id, int restaurantId, List<Category>? categories)
    {
        Id = id;
        RestaurantId = restaurantId;
        Categories = categories;
    } 
}