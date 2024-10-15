using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace host.Models;

public class Menu
{
    [JsonPropertyName("categories")]
    public List<Category?> Categories { get; set; }

    public Menu(List<Category?> categories)
    {
        Categories = categories;
    } 
}