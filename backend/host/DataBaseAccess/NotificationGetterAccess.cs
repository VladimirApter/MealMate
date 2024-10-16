using host.Models;

namespace host.DataBaseAccess;

public class NotificationGetterAccess : DataBaseAccess
{
    private const string NotificationGetterQuery =
        "SELECT username, id FROM notification_getters WHERE id = @notificationGetterId";

    public static NotificationGetter? GetNotificationGetter(int id)
    {
        using var notificationGetterReader = ExecuteReader(NotificationGetterQuery, ("@notificationGetterId", id));

        if (!notificationGetterReader.Read()) return null;

        return new NotificationGetter(
            notificationGetterReader.GetString(0),
            notificationGetterReader.GetInt32(1)
        );
    }
}