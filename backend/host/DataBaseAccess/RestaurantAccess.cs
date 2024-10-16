using host.Models;
namespace host.DataBaseAccess;

public class RestaurantAccess : DataBaseAccess
{
    private const string RestaurantQuery = "SELECT id, name, address FROM restaurants WHERE id = @restaurantId";

    private const string NotificationGetterQuery = @"SELECT ng.username, ng.id
                                           FROM restaurants r
                                           JOIN notification_getters ng ON r.notification_getter_id = ng.id
                                           WHERE r.id = @restaurantId";

    private const string ListTableQuery = "SELECT id FROM tables WHERE restaurant_id = @restaurantId";
    private const string MenuQuery = "SELECT id FROM menus WHERE restaurant_id = @restaurantId";

    public static Restaurant? GetRestaurant(int id)
    {
        using var restaurantReader = ExecuteReader(RestaurantQuery, ("@restaurantId", id));
        if (!restaurantReader.Read()) return null;

        var restaurant = new Restaurant(null, restaurantReader.GetString(1), restaurantReader.GetString(2), null, [], id);

        using var notificationGetterReader = ExecuteReader(NotificationGetterQuery, ("@restaurantId", id));
        if (!notificationGetterReader.Read()) return null;
        restaurant.NotificationGetter = new NotificationGetter(notificationGetterReader.GetString(0), notificationGetterReader.GetInt32(1));

        using var listTableReader = ExecuteReader(ListTableQuery, ("@restaurantId", id));
        while (listTableReader.Read())
        {
            var table = TableAccess.GetTable(listTableReader.GetInt32(0));
            restaurant.Tables.Add(table);
        }

        using var menuReader = ExecuteReader(MenuQuery, ("@restaurantId", id));
        if (!menuReader.Read()) return null;
        restaurant.Menu = MenuAccess.GetMenu(menuReader.GetInt32(0));

        return restaurant;
    }
}