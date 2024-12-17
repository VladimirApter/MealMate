using Domain.DataBaseAccess;

namespace Domain.Models;

public class Client(long? id, string ip) : ITableDataBase
{
    public long? Id { get; set; } = id;
    public string Ip { get; init; } = ip;
}