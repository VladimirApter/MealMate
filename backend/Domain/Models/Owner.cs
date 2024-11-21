using System.Text.Json.Serialization;
using Domain.DataBaseAccess;

namespace Domain.Models;

public class Owner : TgAccount, ITableDataBase
{
    [JsonPropertyName("restaurant_ids")] public List<int>? RestaurantIds { get; set; }

    public Owner(){}
    public Owner(int? id, string username) : base(id, username){}
    public Owner(int? id, string username, List<int>? restaurantIds) : base(id, username)
    {
        RestaurantIds = restaurantIds;
    }
}