using host.DataBaseAccess;

namespace host.Models;

public class NotificationGetter : TgAccount, ITableDataBase
{
    public NotificationGetter(string username, int? id) : base(username, id)
    {
    }
}