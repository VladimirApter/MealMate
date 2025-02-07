using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class Owner : TgAccount, ITableDataBase, ITakeRelatedData
{
    [JsonPropertyName("restaurant_ids")] [NotMapped] public List<long>? RestaurantIds { get; set; }

    public Owner(){}
    public Owner(long? id, string username) : base(id, username){}
    public Owner(long? id, string username, List<long>? restaurantIds) : base(id, username)
    {
        RestaurantIds = restaurantIds;
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        RestaurantIds = await context.Restaurants.Where(r => r.OwnerId == Id).Select(r => r.Id.Value).ToListAsync();
    }
}