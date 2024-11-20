using System.Text.Json.Serialization;
using Domen.DataBaseAccess;

namespace Domen.Models;

public class Owner : TgAccount, ITableDataBase
{
    [JsonPropertyName("restaurant_ids")] public List<int>? RestaurantIds { get; set; }
    
    public Owner(int? id, string username, List<int>? restaurantIds) : base(id, username)
    {
        RestaurantIds = restaurantIds;
    }
}