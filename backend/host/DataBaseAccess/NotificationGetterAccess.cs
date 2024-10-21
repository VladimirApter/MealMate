using host.Models;

namespace host.DataBaseAccess;

public class NotificationGetterAccess : DataBaseAccess
{
    private const string NotificationGetterQuery =
        "SELECT username, id, restaurant_id FROM notification_getters WHERE id = @notificationGetterId";

    private const string InsertCommand =
        @"INSERT INTO [notification_getters] (username, restaurant_id) 
             VALUES (@username, @restaurantId);";

    private const string UpdateCommand = @"UPDATE [notification_getters] 
             SET username = @username 
             WHERE Id = @id;";

    public static NotificationGetter? GetNotificationGetter(int id)
    {
        using var notificationGetterReader = ExecuteReader(NotificationGetterQuery, ("@notificationGetterId", id));

        if (!notificationGetterReader.Read()) return null;

        return new NotificationGetter(
            notificationGetterReader.GetString(0),
            notificationGetterReader.GetInt32(1),
            notificationGetterReader.GetInt32(2)
        );
    }

    public static void AddOrUpdateNotificationGetter(NotificationGetter notificationGetter)
    {
        AddOrUpdateObject(notificationGetter, InsertCommand, UpdateCommand,
            (notificationGetterObj, insertOrUpdateCommand) =>
            {
                insertOrUpdateCommand.Parameters.AddWithValue("@username", notificationGetterObj.Username);
                insertOrUpdateCommand.Parameters.AddWithValue("@restaurantId", notificationGetterObj.RestaurantId);
            });
    }
}