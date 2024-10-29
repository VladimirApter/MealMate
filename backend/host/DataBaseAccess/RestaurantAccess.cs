using host.Models;

namespace host.DataBaseAccess;

public class RestaurantAccess : DataBaseAccess
{
    private const string RestaurantQuery =
        "SELECT id, name, address, owner_id FROM restaurants WHERE id = @restaurantId";

    private const string NotificationGetterQuery =
        "SELECT username, id, restaurant_id FROM notification_getters WHERE id = @restaurantId";
    /*private const string NotificationGetterQuery = @"SELECT ng.username, ng.id
                                           FROM restaurants r
                                           JOIN notification_getters ng ON r.notification_getter_id = ng.id
                                           WHERE r.id = @restaurantId";*/

    private const string ListTableQuery = "SELECT id FROM tables WHERE restaurant_id = @restaurantId";
    private const string MenuQuery = "SELECT id FROM menus WHERE restaurant_id = @restaurantId";

    private const string InsertCommand =
        @"INSERT INTO [restaurants] (owner_id, name, address) 
             VALUES (@ownerId, @name, @address);";

    private const string UpdateCommand = @"UPDATE [restaurants] 
             SET owner_id = @ownerId, name = @name, address = @address
             WHERE Id = @id;";

    public static Restaurant? GetRestaurant(int id)
    {
        using var restaurantReader = ExecuteReader(RestaurantQuery, ("@restaurantId", id));
        if (!restaurantReader.Read()) return null;

        var restaurant = new Restaurant(null, restaurantReader.GetString(1), restaurantReader.GetString(2), null, [],
            id, restaurantReader.GetInt32(3));

        using var notificationGetterReader = ExecuteReader(NotificationGetterQuery, ("@restaurantId", id));
        if (notificationGetterReader.Read())
            restaurant.NotificationGetter =
                new NotificationGetter(notificationGetterReader.GetString(0), notificationGetterReader.GetInt32(1),
                    notificationGetterReader.GetInt32(2));

        using var listTableReader = ExecuteReader(ListTableQuery, ("@restaurantId", id));
        while (listTableReader.Read())
        {
            var table = TableAccess.GetTable(listTableReader.GetInt32(0));
            if (table != null) restaurant.Tables?.Add(table);
        }

        using var menuReader = ExecuteReader(MenuQuery, ("@restaurantId", id));
        if (menuReader.Read())
            restaurant.Menu = MenuAccess.GetMenu(menuReader.GetInt32(0));

        return restaurant;
    }

    public static void AddOrUpdateRestaurant(Restaurant restaurant)
    {
        if (restaurant.Tables != null)
        {
            foreach (var table in restaurant.Tables)
                TableAccess.AddOrUpdateTable(table);
        }

        if (restaurant.Menu != null)
            MenuAccess.AddOrUpdateMenu(restaurant.Menu);

        if (restaurant.NotificationGetter != null)
            NotificationGetterAccess.AddOrUpdateNotificationGetter(restaurant.NotificationGetter);

        AddOrUpdateObject(restaurant, InsertCommand, UpdateCommand,
            (restaurantObj, insertOrUpdateCommand) =>
            {
                insertOrUpdateCommand.Parameters.AddWithValue("@ownerId", restaurantObj.OwnerId);
                insertOrUpdateCommand.Parameters.AddWithValue("@name", restaurantObj.Name);
                insertOrUpdateCommand.Parameters.AddWithValue("@address", restaurantObj.Address);
            });
    }
}