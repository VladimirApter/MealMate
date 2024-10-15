using host.Models;

namespace host.DataBase;

public class NotificationGetterAccess : DataBaseAccess
{
    private const string NotificationGetterQuery = "SELECT email FROM notification_getters WHERE id = @notificationGetterId";
    public static NotificationGetter? GetNotificationGetter(int id)
    {
        using var notificationGetterReader = ExecuteReader(NotificationGetterQuery, ("@notificationGetterId", id));
        
        if (!notificationGetterReader.Read()) return null;

        return new NotificationGetter(
            notificationGetterReader.GetString(0)
        );
    }
}