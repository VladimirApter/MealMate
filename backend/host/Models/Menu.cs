using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace host.Models;

public class Menu
{
    public int Id { get; set; }
    [JsonPropertyName("categories")]
    public List<Category?> Categories { get; set; }

    public Menu(int id, List<Category?> categories)
    {
        Id = id;
        Categories = categories;
    } 
}