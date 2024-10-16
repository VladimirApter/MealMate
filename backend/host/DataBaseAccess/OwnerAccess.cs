using host.Models;
namespace host.DataBaseAccess;

public class OwnerAccess : DataBaseAccess
{
    private const string OwnerQuery = "SELECT name FROM owners WHERE id = @ownerId";
    private const string RestaurantIdsQuery = "SELECT id FROM restaurants WHERE owner_id = @ownerId";
    public static Owner? GetOwner(int id)
    {
        using var ownerReader = ExecuteReader(OwnerQuery, ("@ownerId", id));
        
        if (!ownerReader.Read()) return null;

        var owner = new Owner(ownerReader.GetString(0), []);
        
        using var restaurantIdsReader = ExecuteReader(RestaurantIdsQuery, ("@ownerId", id));
        while (restaurantIdsReader.Read())
        {
            owner.RestaurantIds.Add(restaurantIdsReader.GetInt32(0));
        }
        
        return owner;
    }
}