using Domain.DataBaseAccess;

namespace Domain.Models;

public class Client : ITableDataBase
{
    public long? Id { get; set; }
    public string Ip { get; set; }
    
    public Client(long? id, string ip)
    {
        Id = id;
        Ip = ip;
    }
}