using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace host.Models;

public class Menu
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    [JsonPropertyName("categories")]
    public List<Category> Categories { get; set; }

    public Menu(int id, int restaurantId, List<Category> categories)
    {
        Id = id;
        RestaurantId = restaurantId;
        Categories = categories;
    } 
}